using Blog.Core.IRepository;
using Blog.Core.Repository.Base;
using Blog.Core.Model.Models;
using Blog.Core.IRepository.UnitOfWork;

namespace Blog.Core.Repository
{
    /// <summary>
    /// RoleRepository
    /// </summary>	
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
