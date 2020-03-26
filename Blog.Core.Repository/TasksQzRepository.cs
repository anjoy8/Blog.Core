
using Blog.Core.IRepository;
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.Model.Models;
using Blog.Core.Repository.Base;

namespace Blog.Core.Repository
{
	/// <summary>
	/// TasksQzRepository
	/// </summary>
    public class TasksQzRepository : BaseRepository<TasksQz>, ITasksQzRepository
    {
        public TasksQzRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
                    