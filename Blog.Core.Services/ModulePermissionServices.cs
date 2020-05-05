using Blog.Core.Services.BASE;
using Blog.Core.Model.Models;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.IRepository.Base;

namespace Blog.Core.Services
{	
	/// <summary>
	/// ModulePermissionServices
	/// </summary>	
	public class ModulePermissionServices : BaseServices<ModulePermission>, IModulePermissionServices
    {
        private readonly IBaseRepository<ModulePermission> _dal;

        public ModulePermissionServices(IBaseRepository<ModulePermission> dal)
        {
            base.BaseDal = dal;
            _dal = dal;
        }
       
    }
}
