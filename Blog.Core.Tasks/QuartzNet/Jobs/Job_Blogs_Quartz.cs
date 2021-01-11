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

        public Job_Blogs_Quartz(IBlogArticleServices blogArticleServices, ITasksQzServices tasksQzServices):base(tasksQzServices)
        {
            _blogArticleServices = blogArticleServices; 
        }
        public async Task Execute(IJobExecutionContext context)
        { 
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;
            await ExecuteJob(context, async () => await Run(context, jobId.ObjToInt())); 
        }
        public async Task Run(IJobExecutionContext context, int jobid)
        {
            if (jobid > 0)
            {
                var list = await _blogArticleServices.Query();
            }
        }
    }



}
