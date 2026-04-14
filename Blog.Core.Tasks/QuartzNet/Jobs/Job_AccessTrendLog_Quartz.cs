using Blog.Core.Common.LogHelper;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace Blog.Core.Tasks
{
    public class Job_AccessTrendLog_Quartz : JobBase, IJob
    {
        private readonly IAccessTrendLogServices _accessTrendLogServices;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<Job_AccessTrendLog_Quartz> _logger;

        public Job_AccessTrendLog_Quartz(IAccessTrendLogServices accessTrendLogServices,
            IWebHostEnvironment environment, ITasksQzServices tasksQzServices, ITasksLogServices tasksLogServices,
            ILogger<Job_AccessTrendLog_Quartz> logger)
            : base(tasksQzServices, tasksLogServices)
        {
            _accessTrendLogServices = accessTrendLogServices;
            _environment            = environment;
            _logger                 = logger;
            _tasksQzServices        = tasksQzServices;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var executeLog = await ExecuteJob(context, async () => await Run(context));
        }

        public async Task Run(IJobExecutionContext context)
        {
            // 可以直接获取 JobDetail 的值
            var jobKey = context.JobDetail.Key;
            var jobId  = jobKey.Name;
            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;

            var lastestLogDatetime = (await _accessTrendLogServices.Query(null, d => d.UpdateTime, false))
               .FirstOrDefault()?.UpdateTime;
            if (lastestLogDatetime == null)
            {
                lastestLogDatetime = Convert.ToDateTime("2021-09-01");
            }

            // 重新拉取
            var actUsers = await _accessTrendLogServices.Query(d => d.UserInfo != "", d => d.Count, false);
            actUsers = actUsers.Take(15).ToList();

            List<ActiveUserVM> activeUserVMs = new();
            foreach (var item in actUsers)
            {
                activeUserVMs.Add(new ActiveUserVM()
                {
                    user  = item.UserInfo,
                    count = item.Count
                });
            }

            _logger.LogInformation("Job_AccessTrendLog_Quartz: {ActiveUserVMs}",
                JsonConvert.SerializeObject(activeUserVMs));
        }
    }
}