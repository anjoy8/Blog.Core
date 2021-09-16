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

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace Blog.Core.Tasks
{
    public class Job_AccessTrendLog_Quartz : JobBase, IJob
    {
        private readonly IAccessTrendLogServices _accessTrendLogServices;
        private readonly IWebHostEnvironment _environment;

        public Job_AccessTrendLog_Quartz(IAccessTrendLogServices accessTrendLogServices, ITasksQzServices tasksQzServices, IWebHostEnvironment environment)
        {
            _accessTrendLogServices = accessTrendLogServices;
            _environment = environment;
            _tasksQzServices = tasksQzServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var executeLog = await ExecuteJob(context, async () => await Run(context));
        }
        public async Task Run(IJobExecutionContext context)
        {

            // 可以直接获取 JobDetail 的值
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;
            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;

            var lastestLogDatetime = (await _accessTrendLogServices.Query(null, d => d.Createdate, false)).FirstOrDefault()?.Createdate;
            if (lastestLogDatetime == null)
            {
                lastestLogDatetime = Convert.ToDateTime("2021-08-01");
            }

            var accLogs = GetAccessLogs().Where(d => d.BeginTime.ObjToDate() >= lastestLogDatetime).ToList();

            var accTrendLogs = new List<AccessTrendLog>() { };
            accLogs.ForEach(m =>
            {
                accTrendLogs.Add(new AccessTrendLog()
                {
                    User = m.User,
                    API = m.API,
                    BeginTime = m.BeginTime,
                    Createdate = DateTime.Now,
                    IP = m.IP,
                    RequestMethod = m.RequestMethod?.Length > 50 ? m.RequestMethod.Substring(0, 50) : m.RequestMethod
                });
            });


            if (accTrendLogs.Count > 0)
            {
                var logsIds = await _accessTrendLogServices.Add(accTrendLogs);
            }
        }

        private List<UserAccessFromFIles> GetAccessLogs()
        {
            List<UserAccessFromFIles> userAccessModels = new();
            var accessLogs = LogLock.ReadLog(
                Path.Combine(_environment.ContentRootPath, "Log"), "RecordAccessLogs_", Encoding.UTF8, ReadType.Prefix
                ).ObjToString().TrimEnd(',');

            try
            {
                return JsonConvert.DeserializeObject<List<UserAccessFromFIles>>("[" + accessLogs + "]");
            }
            catch (Exception)
            {
                var accLogArr = accessLogs.Split("\n");
                foreach (var item in accLogArr)
                {
                    if (item.ObjToString() != "")
                    {
                        try
                        {
                            var accItem = JsonConvert.DeserializeObject<UserAccessFromFIles>(item.TrimEnd(','));
                            userAccessModels.Add(accItem);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            }

            return userAccessModels;
        }

    }
    public class UserAccessFromFIles
    {
        public string User { get; set; }
        public string IP { get; set; }
        public string API { get; set; }
        public string BeginTime { get; set; }
        public string OPTime { get; set; }
        public string RequestMethod { get; set; } = "";
        public string Agent { get; set; }
    }

}
