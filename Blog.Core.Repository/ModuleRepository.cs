using Blog.Core.Repository.Base;
using Blog.Core.Model.Models;
using Blog.Core.IRepository;
using Blog.Core.IRepository.UnitOfWork;

namespace Blog.Core.Repository
{
    /// <summary>
    /// ModuleRepository
    /// </summary>	
    public class ModuleRepository : BaseRepository<Module>, IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
