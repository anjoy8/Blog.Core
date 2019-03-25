using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.AuthHelper.OverWrite;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize("Permission")]
    public class UserController : ControllerBase
    {
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        public UserController(ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices)
        {
            _sysUserInfoServices = sysUserInfoServices;
            _userRoleServices = userRoleServices;
            _roleServices = roleServices;
        }

        // GET: api/User
        [HttpGet]
        public async Task<MessageModel<PageModel<sysUserInfo>>> Get(int page = 1, string key = "")
        {
            var data = new MessageModel<PageModel<sysUserInfo>>();
            int intTotalCount = 50;
            int totalCount = 0;
            int pageCount = 1;
            List<sysUserInfo> sysUserInfos = new List<sysUserInfo>();

            sysUserInfos = await _sysUserInfoServices.Query(a => a.tdIsDelete != true && a.uStatus >= 0);

            if (!string.IsNullOrEmpty(key))
            {
                sysUserInfos = sysUserInfos.Where(t => (t.uLoginName != null && t.uLoginName.Contains(key)) || (t.uRealName != null && t.uRealName.Contains(key))).ToList();
            }


            //筛选后的数据总数
            totalCount = sysUserInfos.Count;
            //筛选后的总页数
            pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intTotalCount.ObjToDecimal())).ObjToInt();

            sysUserInfos = sysUserInfos.OrderByDescending(d => d.uID).Skip((page - 1) * intTotalCount).Take(intTotalCount).ToList();


            var allUserRoles = await _userRoleServices.Query(d => d.IsDeleted == false);
            var allRoles = await _roleServices.Query(d => d.IsDeleted == false);
            foreach (var item in sysUserInfos)
            {
                item.uLoginPWD = "no see me";
                item.RID = (allUserRoles.FirstOrDefault(d => d.UserId == item.uID)?.RoleId).ObjToInt();
                item.RoleName = allRoles.FirstOrDefault(d => d.Id==item.RID)?.Name;
            }

            return new MessageModel<PageModel<sysUserInfo>>()
            {
                msg = "获取成功",
                success = totalCount >= 0,
                response = new PageModel<sysUserInfo>()
                {
                    page = page,
                    pageCount = pageCount,
                    dataCount = totalCount,
                    data = sysUserInfos,
                }
            };

        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return "value";
        }

        // GET: api/User/5
        /// <summary>
        /// 获取用户详情根据token
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<sysUserInfo>> GetInfoByToken(string token)
        {
            var data = new MessageModel<sysUserInfo>();
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

        // POST: api/User
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] sysUserInfo sysUserInfo)
        {
            var data = new MessageModel<string>();

            var id = await _sysUserInfoServices.Add(sysUserInfo);
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        // PUT: api/User/5
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] sysUserInfo sysUserInfo)
        {
            var data = new MessageModel<string>();
            if (sysUserInfo != null && sysUserInfo.uID > 0)
            {
                if (sysUserInfo.RID > 0)
                {
                    var usrerole = await _userRoleServices.Query(d => d.UserId == sysUserInfo.uID && d.RoleId == sysUserInfo.RID);
                    if (usrerole.Count == 0)
                    {
                        await _userRoleServices.Add(new UserRole(sysUserInfo.uID, sysUserInfo.RID));
                    }
                }

                data.success = await _sysUserInfoServices.Update(sysUserInfo);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = sysUserInfo?.uID.ObjToString();
                }
            }

            return data;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var userDetail = await _sysUserInfoServices.QueryById(id);
                userDetail.tdIsDelete = true;
                data.success = await _sysUserInfoServices.Update(userDetail);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = userDetail?.uID.ObjToString();
                }
            }

            return data;
        }
    }
}
