using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;
using Blog.Core.IServices;
using Blog.Core.IRepository;
using System.Threading.Tasks;
using System.Linq;
using Blog.Core.IRepository.Base;

namespace Blog.Core.FrameWork.Services
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>	
    public class SysUserInfoServices : BaseServices<sysUserInfo>, ISysUserInfoServices
    {
        private readonly IBaseRepository<sysUserInfo> _dal;
        private readonly IBaseRepository<UserRole> _userRoleServices;
        private readonly IBaseRepository<Role> _roleRepository;

        public SysUserInfoServices(IBaseRepository<sysUserInfo> dal, IBaseRepository<UserRole> userRoleServices, IBaseRepository<Role> roleRepository)
        {
            _dal = dal;
            this._userRoleServices = userRoleServices;
            _roleRepository = roleRepository;
            this._roleRepository = roleRepository;
            base.BaseDal = dal;
            _userRoleServices = userRoleServices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<sysUserInfo> SaveUserInfo(string loginName, string loginPwd)
        {
            sysUserInfo sysUserInfo = new sysUserInfo(loginName, loginPwd);
            sysUserInfo model = new sysUserInfo();
            var userList = await base.Query(a => a.uLoginName == sysUserInfo.uLoginName && a.uLoginPWD == sysUserInfo.uLoginPWD);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(sysUserInfo);
                model = await base.QueryById(id);
            }

            return model;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<string> GetUserRoleNameStr(string loginName, string loginPwd)
        {
            string roleName = "";
            var user = (await base.Query(a => a.uLoginName == loginName && a.uLoginPWD == loginPwd)).FirstOrDefault();
            var roleList = await _roleRepository.Query(a => a.IsDeleted==false);
            if (user != null)
            {
                var userRoles = await _userRoleServices.Query(ur => ur.UserId == user.uID);
                if (userRoles.Count > 0)
                {
                    var arr = userRoles.Select(ur => ur.RoleId.ObjToString()).ToList();
                    var roles = roleList.Where(d => arr.Contains(d.Id.ObjToString()));

                    roleName = string.Join(',', roles.Select(r => r.Name).ToArray());
                }
            }
            return roleName;
        }
    }
}

//----------sysUserInfo结束----------
