using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;
using System.Threading.Tasks;

namespace Blog.Core.IServices
{	
	/// <summary>
	/// RoleServices
	/// </summary>	
    public interface IRoleServices :IBaseServices<Role>
	{
        Task<Role> SaveRole(string roleName);
        Task<string> GetRoleNameByRid(int rid);

        /// <summary>
        /// 是否存在子类
        /// </summary>
        /// <param name="id">当前id</param>
        /// <returns></returns>
        Task<bool> ExistsChild(int id);
    }
}
