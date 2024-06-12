using Blog.Core.IServices;
using Quartz;
using System;
using System.Threading.Tasks;
using Blog.Core.Model.Models;
using Blog.Core.Repository.UnitOfWorks;
using SqlSugar;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace Blog.Core.Tasks
{
    [DisallowConcurrentExecution]
    public class Job_Blogs_Quartz : JobBase, IJob
    {
        private readonly IBlogArticleServices _blogArticleServices;
        private readonly IGuestbookServices _guestbookServices;
        private readonly IUnitOfWorkManage _uowm;
        private readonly ISqlSugarClient _db;
        private SqlSugarScope db => _db as SqlSugarScope;

        public Job_Blogs_Quartz(IBlogArticleServices blogArticleServices, ITasksQzServices tasksQzServices, ITasksLogServices tasksLogServices,
            IGuestbookServices guestbookServices, IUnitOfWorkManage uowm, ISqlSugarClient db)
            : base(tasksQzServices, tasksLogServices)
        {
            _blogArticleServices = blogArticleServices;
            _guestbookServices = guestbookServices;
            _uowm = uowm;
            this._db = db;
        }

        /// <summary>
        /// 直接写就没有锁库 上下文ContextID一样
        /// </summary>
        /// <param name="context"></param>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                db.BeginTran();
                Console.WriteLine(_uowm.GetDbClient().ContextID);
                await db.Insertable(new Guestbook()
                {
                    username = "bbb",
                    blogId = 1,
                    createdate = DateTime.Now,
                    isshow = true
                }).ExecuteReturnSnowflakeIdAsync();
                await db.Insertable(new PasswordLib()
                {
                    PLID = SnowFlakeSingle.Instance.NextId(),
                    IsDeleted = false,
                    plAccountName = "aaa",
                    plCreateTime = DateTime.Now
                }).ExecuteReturnSnowflakeIdAsync();

                db.BeginTran();
                Console.WriteLine(db.ContextID);
                await db.Insertable(new PasswordLib()
                {
                    PLID = SnowFlakeSingle.Instance.NextId(),
                    IsDeleted = false,
                    plAccountName = "aaa",
                    plCreateTime = DateTime.Now
                }).ExecuteReturnSnowflakeIdAsync();

                db.CommitTran();

                Console.WriteLine(db.ContextID);
                db.CommitTran();
                Console.WriteLine("完成");
            }
            catch (Exception e)
            {
                db.RollbackTran();
            }
        }

        /// <summary>
        /// 但是调用其他类方法 上下文ContextID就不一样
        /// </summary>
        /// <param name="context"></param>
        public async Task Execute2(IJobExecutionContext context)
        {
            await _guestbookServices.TestTranPropagationTran3();

            //var executeLog = await ExecuteJob(context, async () => await Run(context));
        }

        public async Task Run(IJobExecutionContext context)
        {
            System.Console.WriteLine($"Job_Blogs_Quartz 执行 {DateTime.Now.ToShortTimeString()}");
            var list = await _blogArticleServices.Query();
            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;
            //int jobId = data.GetInt("JobParam");
        }
    }
}