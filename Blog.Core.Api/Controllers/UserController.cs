using AutoMapper;
using Blog.Core.AuthHelper.OverWrite;
using Blog.Core.Common.Helper;
using Blog.Core.Common.HttpContextUser;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using Blog.Core.Repository.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class UserController : BaseApiController
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly IUser _user;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public IHttpContextAccessor _httpContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManage"></param>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        /// <param name="departmentServices"></param>
        /// <param name="user"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param
        /// <param name="httpContext"></param>
        public UserController(IUnitOfWorkManage unitOfWorkManage, ISysUserInfoServices sysUserInfoServices,
            IUserRoleServices userRoleServices,
            IRoleServices roleServices,
            IDepartmentServices departmentServices,
            IUser user, IMapper mapper, ILogger<UserController> logger
            ,IHttpContextAccessor httpContext)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _sysUserInfoServices = sysUserInfoServices;
            _userRoleServices = userRoleServices;
            _roleServices = roleServices;
            _departmentServices = departmentServices;
            _user = user;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContext;

        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<SysUserInfo>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            int intPageSize = 50;


            var data = await _sysUserInfoServices.QueryPage(a => a.IsDeleted != true && a.Status >= 0 && ((a.LoginName != null && a.LoginName.Contains(key)) || (a.RealName != null && a.RealName.Contains(key))), page, intPageSize, " Id desc ");


            #region MyRegion

            // 这里可以封装到多表查询，此处简单处理
            var allUserRoles = await _userRoleServices.Query(d => d.IsDeleted == false);
            var allRoles = await _roleServices.Query(d => d.IsDeleted == false);
            var allDepartments = await _departmentServices.Query(d => d.IsDeleted == false);

            var sysUserInfos = data.data;
            foreach (var item in sysUserInfos)
            {
                var currentUserRoles = allUserRoles.Where(d => d.UserId == item.Id).Select(d => d.RoleId).ToList();
                item.RIDs = currentUserRoles;
                item.RoleNames = allRoles.Where(d => currentUserRoles.Contains(d.Id)).Select(d => d.Name).ToList();
                var departmentNameAndIds = GetFullDepartmentName(allDepartments, item.DepartmentId);
                item.DepartmentName = departmentNameAndIds.Item1;
                item.Dids = departmentNameAndIds.Item2;
            }

            data.data = sysUserInfos;

            #endregion


            return Success(data.ConvertTo<SysUserInfo>(_mapper));
        }

        private (string, List<long>) GetFullDepartmentName(List<Department> departments, long departmentId)
        {
            var departmentModel = departments.FirstOrDefault(d => d.Id == departmentId);
            if (departmentModel == null)
            {
                return ("", new List<long>());
            }

            var pids = departmentModel.CodeRelationship?.TrimEnd(',').Split(',').Select(d => d.ObjToLong()).ToList();
            pids.Add(departmentModel.Id);
            var pnams = departments.Where(d => pids.Contains(d.Id)).ToList().Select(d => d.Name).ToArray();
            var fullName = string.Join("/", pnams);

            return (fullName, pids);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public string Get(string id)
        {
            _logger.LogError("test wrong");
            return "value";
        }

        // GET: api/User/5
        /// <summary>
        /// 获取用户详情根据token
        /// 【无权限】
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<SysUserInfo>> GetInfoByToken(string token)
        {
            var data = new MessageModel<SysUserInfo>();
            if (!string.IsNullOrEmpty(token))
            {
                var tokenModel = JwtHelper.SerializeJwt(token);
                if (tokenModel != null && tokenModel.Uid > 0)
                {
                    var userinfo = await _sysUserInfoServices.QueryById(tokenModel.Uid);
                    if (userinfo != null)
                    {
                        data.response = userinfo;
                        data.success = true;
                        data.msg = "获取成功";
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] SysUserInfo sysUserInfo)
        {
            var data = new MessageModel<string>();

            sysUserInfo.LoginPWD = MD5Helper.MD5Encrypt32(sysUserInfo.LoginPWD);
            sysUserInfo.Remark = _user.Name;

            var id = await _sysUserInfoServices.Add(sysUserInfo);
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        /// <summary>
        /// 更新用户与角色
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] SysUserInfo sysUserInfo)
        {
            // 这里使用事务处理
            var data = new MessageModel<string>();

            var oldUser = await _sysUserInfoServices.QueryById(sysUserInfo.Id);
            if (oldUser is not { Id: > 0 })
            {
                return Failed<string>("用户不存在或已被删除");
            }

            try
            {
                if (sysUserInfo.LoginPWD != oldUser.LoginPWD)
                {
                    oldUser.CriticalModifyTime = DateTime.Now;
                }

                _mapper.Map(sysUserInfo, oldUser);

                _unitOfWorkManage.BeginTran();
                // 无论 Update Or Add , 先删除当前用户的全部 U_R 关系
                var usreroles = (await _userRoleServices.Query(d => d.UserId == oldUser.Id));
                if (usreroles.Any())
                {
                    var ids = usreroles.Select(d => d.Id.ToString()).ToArray();
                    var isAllDeleted = await _userRoleServices.DeleteByIds(ids);
                    if (!isAllDeleted)
                    {
                        return Failed("服务器更新异常");
                    }
                }

                // 然后再执行添加操作
                if (sysUserInfo.RIDs.Count > 0)
                {
                    var userRolsAdd = new List<UserRole>();
                    sysUserInfo.RIDs.ForEach(rid => { userRolsAdd.Add(new UserRole(oldUser.Id, rid)); });

                    var oldRole = usreroles.Select(s => s.RoleId).OrderBy(i => i).ToArray();
                    var newRole = userRolsAdd.Select(s => s.RoleId).OrderBy(i => i).ToArray();
                    if (!oldRole.SequenceEqual(newRole))
                    {
                        oldUser.CriticalModifyTime = DateTime.Now;
                    }

                    await _userRoleServices.Add(userRolsAdd);
                }

                data.success = await _sysUserInfoServices.Update(oldUser);

                _unitOfWorkManage.CommitTran();

                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = oldUser.Id.ObjToString();
                }
            }
            catch (Exception e)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.LogError(e, e.Message);
            }

            return data;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(long id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _sysUserInfoServices.QueryById(id);
                userDetail.IsDeleted = true;
                data.success = await _sysUserInfoServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = userDetail?.Id.ObjToString();
                }
            }

            return data;
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> ResetPass([FromBody] SysUserInfo sysUserInfo)
        {
            //重置密码

            var oldUser = await _sysUserInfoServices.QueryById(sysUserInfo.Id);
            if (oldUser == null || oldUser.IsDeleted)
            {
                return Failed<string>("用户不存在或已被删除");
            }
            oldUser.LoginPWD = MD5Helper.MD5Encrypt32(sysUserInfo.LoginPWD);
            await _sysUserInfoServices.Update(oldUser);
            return Success<string>("重置成功");
        }

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<MessageModel<string>> RefreshMyLogo([FromBody] SysUserInfo sysUserInfo)
        {
            //重置密码 登录既可修改
            var uid = _user.ID;
            var oldUser = await _sysUserInfoServices.QueryById(uid);
            if (oldUser == null || oldUser.IsDeleted)
            {
                return Failed<string>("用户不存在或已被删除");
            }
            oldUser.logo = sysUserInfo.logo;
            await _sysUserInfoServices.Update(oldUser);
            return Success<string>("更新成功");
        }

        /// <summary>
        /// 重置我的密码
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<MessageModel<string>> ResetMyPass([FromBody] SysUserInfo sysUserInfo)
        {
            //重置密码 登录既可修改

            var uid = _user.ID;
            var oldUser = await _sysUserInfoServices.QueryById(uid);
            if (oldUser == null || oldUser.IsDeleted)
            {
                return Failed<string>("用户不存在或已被删除");
            }
            oldUser.LoginPWD = MD5Helper.MD5Encrypt32(sysUserInfo.LoginPWD);
            await _sysUserInfoServices.Update(oldUser);
            return Success<string>("重置成功");
        }
        /// <summary>
        /// 更新我的资料
        /// </summary>
        /// <param name="sysUserInfo"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<MessageModel<string>> PutMyInfo([FromBody] SysUserInfo sysUserInfo)
        {

            var uid = _user.ID;

            var data = new MessageModel<string>();

            //登录账号判断
            var hasUser = await _sysUserInfoServices.Query(t => t.LoginName == sysUserInfo.LoginName && t.IsDeleted == false && t.Id != uid);
            if (hasUser.Count > 0)
            {
                return Failed<string>($"登录名:{sysUserInfo.LoginName}已存在,请重新填写!");
            }

            var oldUser = await _sysUserInfoServices.QueryById(uid);
            if (oldUser == null || oldUser.IsDeleted)
            {
                return Failed<string>("用户不存在或已被删除");
            }

            oldUser.RealName = sysUserInfo.RealName;
            oldUser.LoginName = sysUserInfo.LoginName;
            oldUser.Sex = sysUserInfo.Sex;
            oldUser.Age = sysUserInfo.Age;
            oldUser.Birth = sysUserInfo.Birth;
            oldUser.Address = sysUserInfo.Address;
            oldUser.Remark = sysUserInfo.Remark;
            await _sysUserInfoServices.Update(oldUser);

            return Success<string>("更新成功");
        }
    }
}