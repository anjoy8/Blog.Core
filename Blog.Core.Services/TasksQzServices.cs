
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;

namespace Blog.Core.Services
{
    public partial class TasksQzServices : BaseServices<TasksQz>, ITasksQzServices
    {
        ITasksQzRepository _dal;
        public TasksQzServices(ITasksQzRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
                    