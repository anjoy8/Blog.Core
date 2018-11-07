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


    }
}
