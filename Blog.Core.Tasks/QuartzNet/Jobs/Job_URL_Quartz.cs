using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到(任务类要去JobSetup添加注入)
/// </summary>
namespace Blog.Core.Tasks
{
    public class Job_URL_Quartz : JobBase, IJob
    {
        private readonly ILogger<Job_URL_Quartz> _logger;

        public Job_URL_Quartz(ILogger<Job_URL_Quartz> logger, ITasksQzServices tasksQzServices, ITasksLogServices tasksLogServices)
            : base(tasksQzServices, tasksLogServices)
        {
            _tasksQzServices = tasksQzServices;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        { 
            // 可以直接获取 JobDetail 的值
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name; 
            var executeLog = await ExecuteJob(context, async () => await Run(context, jobId.ObjToInt()));

        }
        public async Task Run(IJobExecutionContext context, int jobid)
        { 
            if (jobid > 0)
            {
                JobDataMap data = context.JobDetail.JobDataMap;
                string pars = data.GetString("JobParam");
                if (!string.IsNullOrWhiteSpace(pars))
                {
                    var log = await HttpHelper.GetAsync(pars);
                    _logger.LogInformation(log);
                }
            }
        }
    }



}
