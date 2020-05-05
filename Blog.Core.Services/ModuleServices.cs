using Blog.Core.Services.BASE;
using Blog.Core.Model.Models;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.IRepository.Base;

namespace Blog.Core.Services
{	
	/// <summary>
	/// ModuleServices
	/// </summary>	
	public class ModuleServices : BaseServices<Module>, IModuleServices
    {
        private readonly IBaseRepository<Module> _dal;

        public ModuleServices(IBaseRepository<Module> dal)
        {
            base.BaseDal = dal;
            _dal = dal;
        }
       
    }
}
