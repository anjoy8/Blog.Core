using Blog.Core.IRepository;
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.Model.Models;
using Blog.Core.Repository.Base;

namespace Blog.Core.Repository
{
    public class TopicRepository : BaseRepository<Topic>, ITopicRepository
    {
        public TopicRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
