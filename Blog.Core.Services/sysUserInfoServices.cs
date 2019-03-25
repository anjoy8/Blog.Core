using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;
using Blog.Core.IServices;
using Blog.Core.IRepository;
using System.Threading.Tasks;
using System.Linq;

namespace Blog.Core.FrameWork.Services
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>	
    public class sysUserInfoServices : BaseServices<sysUserInfo>, IsysUserInfoServices
    {

        IsysUserInfoRepository dal;
        IUserRoleServices userRoleServices;
        IRoleRepository roleRepository;
        public sysUserInfoServices(IsysUserInfoRepository dal, IUserRoleServices userRoleServices, IRoleRepository roleRepository)
        {
            this.dal = dal;
            this.userRoleServices = userRoleServices;
            this.roleRepository = roleRepository;
            base.baseDal = dal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPWD"></param>
        /// <returns></returns>
        public async Task<sysUserInfo> SaveUserInfo(string loginName, string loginPWD)
        {
            sysUserInfo sysUserInfo = new sysUserInfo(loginName, loginPWD);
            sysUserInfo model = new sysUserInfo();
            var userList = await base.Query(a => a.uLoginName == sysUserInfo.uLoginName && a.uLoginPWD == sysUserInfo.uLoginPWD);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(sysUserInfo);
                model = await base.QueryByID(id);
            }

            return model;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPWD"></param>
        /// <returns></returns>
        public async Task<string> GetUserRoleNameStr(string loginName, string loginPWD)
        {
            string roleName = "";
            var user = (await base.Query(a => a.uLoginName == loginName && a.uLoginPWD == loginPWD)).FirstOrDefault();
            if (user != null)
            {
                var userRoles = await userRoleServices.Query(ur => ur.UserId == user.uID);
                if (userRoles.Count > 0)
                {
                    var roles = await roleRepository.QueryByIDs(userRoles.Select(ur => ur.RoleId.ObjToString()).ToArray());

                    roleName = string.Join(',', roles.Select(r => r.Name).ToArray());
                }
            }
            return roleName;
        }
    }
}

//----------sysUserInfo结束----------
