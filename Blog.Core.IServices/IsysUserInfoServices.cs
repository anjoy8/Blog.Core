    

using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;
using System.Threading.Tasks;

namespace Blog.Core.IServices
{	
	/// <summary>
	/// sysUserInfoServices
	/// </summary>	
    public interface IsysUserInfoServices :IBaseServices<sysUserInfo>
	{
        Task<sysUserInfo> SaveUserInfo(string loginName, string loginPWD);
        Task<string> GetUserRoleNameStr(string loginName, string loginPWD);
    }
}
