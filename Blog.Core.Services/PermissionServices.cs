using Blog.Core.Services.BASE;
using Blog.Core.Model.Models;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.IServices.BASE;
using Blog.Core.IRepository.Base;

namespace Blog.Core.Services
{	
	/// <summary>
	/// PermissionServices
	/// </summary>	
	public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {
        private readonly IBaseRepository<Permission> _dal;

        public PermissionServices(IBaseRepository<Permission> dal)
        {
            base.BaseDal = dal;
            _dal = dal;
        }
       
    }
}
