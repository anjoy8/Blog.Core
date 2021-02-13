using Blog.Core.Common.Helper;
using Blog.Core.Common.LogHelper;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Hosting;
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
    public class Job_OperateLog_Quartz : JobBase, IJob
    {
        private readonly IOperateLogServices _operateLogServices; 
        private readonly IWebHostEnvironment _environment;

        public Job_OperateLog_Quartz(IOperateLogServices operateLogServices, ITasksQzServices tasksQzServices, IWebHostEnvironment environment) : base(tasksQzServices)
        {
            _operateLogServices = operateLogServices; 
            _environment = environment;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // 可以直接获取 JobDetail 的值
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name;

            var executeLog = await ExecuteJob(context, async () => await Run(context, jobId.ObjToInt()));

            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;
        }
        public async Task Run(IJobExecutionContext context, int jobid)
        {
            List<LogInfo> excLogs = new List<LogInfo>();
            var exclogContent = LogLock.ReadLog(Path.Combine(_environment.ContentRootPath, "Log"), $"GlobalExceptionLogs_{DateTime.Now.ToString("yyyMMdd")}.log", Encoding.UTF8);

            if (!string.IsNullOrEmpty(exclogContent))
            {
                excLogs = exclogContent.Split("--------------------------------")
                             .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                             .Select(d => new LogInfo
                             {
                                 Datetime = (d.Split("|")[0]).Split(',')[0].ObjToDate(),
                                 Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                                 LogColor = "EXC",
                                 Import = 9,
                             }).ToList();
            }

            var filterDatetime = DateTime.Now.AddHours(-1);
            excLogs = excLogs.Where(d => d.Datetime >= filterDatetime).ToList();

            var operateLogs = new List<OperateLog>() { };
            excLogs.ForEach(m =>
            {
                operateLogs.Add(new OperateLog()
                {
                    LogTime = m.Datetime,
                    Description = m.Content,
                    IPAddress = m.IP,
                    UserId = 0,
                    IsDeleted = false,
                });
            });


            if (operateLogs.Count > 0)
            {
                var logsIds = await _operateLogServices.Add(operateLogs);
            }

            if (jobid > 0)
            {
                var model = await _tasksQzServices.QueryById(jobid);
                if (model != null)
                {
                    var list = await _operateLogServices.Query(d => d.IsDeleted == false);
                    model.RunTimes += 1;
                    var separator = "<br>";
                    model.Remark =
                        $"【{DateTime.Now}】执行任务【Id：{context.JobDetail.Key.Name}，组别：{context.JobDetail.Key.Group}】【执行成功】:异常数{list.Count}{separator}"
                        + string.Join(separator, StringHelper.GetTopDataBySeparator(model.Remark, separator, 9));

                    await _tasksQzServices.Update(model);
                }
            }
        }
    }



}
