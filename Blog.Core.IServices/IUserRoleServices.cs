using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;
using System.Threading.Tasks;

namespace Blog.Core.IServices
{	
	/// <summary>
	/// UserRoleServices
	/// </summary>	
    public interface IUserRoleServices :IBaseServices<UserRole>
	{

        Task<UserRole> SaveUserRole(int uid, int rid);
    }
}

