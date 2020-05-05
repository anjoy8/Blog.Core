
using Blog.Core.IRepository;
using Blog.Core.IRepository.Base;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;

namespace Blog.Core.Services
{
    public partial class TasksQzServices : BaseServices<TasksQz>, ITasksQzServices
    {
        private readonly IBaseRepository<TasksQz> _dal;

        public TasksQzServices(IBaseRepository<TasksQz> dal)
        {
            base.BaseDal = dal;
            _dal = dal;
        }

    }
}
                    