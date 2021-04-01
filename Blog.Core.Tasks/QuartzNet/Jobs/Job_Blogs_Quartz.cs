using Blog.Core.IServices;
using Quartz;
using System.Threading.Tasks;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace Blog.Core.Tasks
{
    public class Job_Blogs_Quartz : JobBase, IJob
    {
        private readonly IBlogArticleServices _blogArticleServices;

        public Job_Blogs_Quartz(IBlogArticleServices blogArticleServices, ITasksQzServices tasksQzServices)
        {
            _blogArticleServices = blogArticleServices;
            _tasksQzServices = tasksQzServices;
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
