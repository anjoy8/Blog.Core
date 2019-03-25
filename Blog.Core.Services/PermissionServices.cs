using Blog.Core.Services.BASE;
using Blog.Core.Model.Models;
using Blog.Core.IRepository;
using Blog.Core.IServices;

namespace Blog.Core.Services
{	
	/// <summary>
	/// PermissionServices
	/// </summary>	
	public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {
	
        IPermissionRepository _dal;
        public PermissionServices(IPermissionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
