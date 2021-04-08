using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace Blog.Core.Tasks
{
    public class Job_Blogs_Quartz : JobBase, IJob
    {
        private readonly IBlogArticleServices _blogArticleServices;  

        public Job_Blogs_Quartz(ITasksQzServices tasksQzServices, ILogger<JobBase> logger, IBlogArticleServices blogArticleServices):base(tasksQzServices, logger)
        { 
            _tasksQzServices = tasksQzServices;
            _logger = logger;
            _blogArticleServices = blogArticleServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var executeLog = await ExecuteJob(context, async () => await Run(context));
        }
        public async Task Run(IJobExecutionContext context)
        {
            var list = await _blogArticleServices.Query();
            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;
            //int jobId = data.GetInt("JobParam");
        }
    }
}
