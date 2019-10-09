using Blog.Core.IRepository;
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.Model.Models;
using Blog.Core.Repository.Base;

namespace Blog.Core.Repository
{
    public partial class PasswordLibRepository : BaseRepository<PasswordLib>, IPasswordLibRepository
    {
        public PasswordLibRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
