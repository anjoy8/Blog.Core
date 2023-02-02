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

            var lastestLogDatetime = (await _accessTrendLogServices.Query(null, d => d.UpdateTime, false)).FirstOrDefault()?.UpdateTime;
            if (lastestLogDatetime == null)
            {
                lastestLogDatetime = Convert.ToDateTime("2021-09-01");
            }

            var accLogs = GetAccessLogs().Where(d => d.User != "" && d.BeginTime.ObjToDate() >= lastestLogDatetime).ToList();
            var logUpdate = DateTime.Now;

            var activeUsers = (from n in accLogs
                               group n by new { n.User } into g
                               select new ActiveUserVM
                               {
                                   user = g.Key.User,
                                   count = g.Count(),
                               }).ToList();

            foreach (var item in activeUsers)
            {
                var user = (await _accessTrendLogServices.Query(d => d.User != "" && d.User == item.user)).FirstOrDefault();
                if (user != null)
                {
                    user.Count += item.count;
                    user.UpdateTime = logUpdate;
                    await _accessTrendLogServices.Update(user);
                }
                else
                {
                    await _accessTrendLogServices.Add(new AccessTrendLog()
                    {
                        Count = item.count,
                        UpdateTime = logUpdate,
                        User = item.user
                    });
                }
            }

            // 重新拉取
            var actUsers = await _accessTrendLogServices.Query(d => d.User != "", d => d.Count, false);
            actUsers = actUsers.Take(15).ToList();

            List<ActiveUserVM> activeUserVMs = new();
            foreach (var item in actUsers)
            {
                activeUserVMs.Add(new ActiveUserVM()
                {
                    user = item.User,
                    count = item.Count
                });
            }

            Parallel.For(0, 1, e =>
            {
                LogLock.OutLogAOP("ACCESSTRENDLOG","",new string[] { activeUserVMs.GetType().ToString(), JsonConvert.SerializeObject(activeUserVMs) }, false);
            });
        }

        private List<UserAccessFromFIles> GetAccessLogs()
        {
            List<UserAccessFromFIles> userAccessModels = new();
            var accessLogs = LogLock.ReadLog(
                Path.Combine(_environment.ContentRootPath, "Log"), "RecordAccessLogs_", Encoding.UTF8, ReadType.Prefix, 2
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
