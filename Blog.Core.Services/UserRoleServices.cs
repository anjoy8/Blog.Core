using Blog.Core.IServices;
using Blog.Core.FrameWork.IRepository;
using Blog.Core.Services.BASE;
using Blog.Core.Model.Models;
using System.Threading.Tasks;
using System.Linq;


namespace Blog.Core.Services
{	
	/// <summary>
	/// UserRoleServices
	/// </summary>	
	public class UserRoleServices : BaseServices<UserRole>, IUserRoleServices
    {
	
        IUserRoleRepository dal;
        public UserRoleServices(IUserRoleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public async Task<UserRole> SaveUserRole(int uid, int rid)
        {
            UserRole userRole = new UserRole(uid, rid);

            UserRole model = new UserRole();
            var userList = await dal.Query(a => a.UserId == userRole.UserId && a.RoleId == userRole.RoleId);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await dal.Add(userRole);
                model = await dal.QueryByID(id);
            }

            return model;

        }
    }
}
