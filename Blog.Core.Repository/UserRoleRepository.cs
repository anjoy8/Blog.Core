using Blog.Core.FrameWork.IRepository;
using Blog.Core.Repository.Base;
using Blog.Core.Model.Models;
using Blog.Core.IRepository.UnitOfWork;

namespace Blog.Core.Repository
{
    /// <summary>
    /// UserRoleRepository
    /// </summary>	
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
