using Blog.Core.Services.BASE;
using Blog.Core.Model.Models;
using Blog.Core.IRepository;
using Blog.Core.IServices;

namespace Blog.Core.Services
{	
	/// <summary>
	/// ModuleServices
	/// </summary>	
	public class ModuleServices : BaseServices<Module>, IModuleServices
    {
	
        IModuleRepository dal;
        public ModuleServices(IModuleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}
