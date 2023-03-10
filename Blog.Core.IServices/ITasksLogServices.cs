
using System;
using System.Threading.Tasks;
using Blog.Core.IServices.BASE;
using Blog.Core.Model;
using Blog.Core.Model.Models;

namespace Blog.Core.IServices
{	
	/// <summary>
	/// ITasksLogServices
	/// </summary>	
    public interface ITasksLogServices :IBaseServices<TasksLog>
	{
		public Task<PageModel<TasksLog>> GetTaskLogs(int jobId, int page, int intPageSize,DateTime? runTime,DateTime? endTime);
        public Task<object> GetTaskOverview(int jobId, DateTime? runTime, DateTime? endTime, string type);
    }
}
                    