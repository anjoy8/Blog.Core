using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Blog.Core.Common.LogHelper;
using Blog.Core.Hubs;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Extensions.Middlewares;

namespace Blog.Core.Controllers
{
    [Route("api/[Controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class MonitorController : BaseApiController
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IWebHostEnvironment _env;
        private readonly IApplicationUserServices _applicationUserServices;
        private readonly ILogger<MonitorController> _logger;

        public MonitorController(IHubContext<ChatHub> hubContext, IWebHostEnvironment env, IApplicationUserServices applicationUserServices, ILogger<MonitorController> logger)
        {
            _hubContext = hubContext;
            _env = env;
            _applicationUserServices = applicationUserServices;
            _logger = logger;
        }

        /// <summary>
        /// 服务器配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageModel<ServerViewModel> Server()
        {
            return Success(new ServerViewModel()
            {
                EnvironmentName = _env.EnvironmentName,
                OSArchitecture = RuntimeInformation.OSArchitecture.ObjToString(),
                ContentRootPath = _env.ContentRootPath,
                WebRootPath = _env.WebRootPath,
                FrameworkDescription = RuntimeInformation.FrameworkDescription,
                MemoryFootprint = (Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2") + " MB",
                WorkingTime = DateHelper.TimeSubTract(DateTime.Now, Process.GetCurrentProcess().StartTime)
            }, "获取服务器配置信息成功");
        }


        /// <summary>
        /// SignalR send data
        /// </summary>
        /// <returns></returns>
        // GET: api/Logs
        [HttpGet]
        public MessageModel<List<LogInfo>> Get()
        {
            if (Appsettings.app(new string[] { "Middleware", "SignalRSendLog", "Enabled" }).ObjToBool())
            {
                _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData()).Wait();
            }
            return Success<List<LogInfo>>(null, "执行成功");
        }



        [HttpGet]
        public MessageModel<RequestApiWeekView> GetRequestApiinfoByWeek()
        {
            return Success(LogLock.RequestApiinfoByWeek(), "成功");
        }

        [HttpGet]
        public MessageModel<AccessApiDateView> GetAccessApiByDate()
        {
            //return new MessageModel<AccessApiDateView>()
            //{
            //    msg = "获取成功",
            //    success = true,
            //    response = LogLock.AccessApiByDate()
            //};

            return Success(LogLock.AccessApiByDate(), "获取成功");
        }

        [HttpGet]
        public MessageModel<AccessApiDateView> GetAccessApiByHour()
        {
            //return new MessageModel<AccessApiDateView>()
            //{
            //    msg = "获取成功",
            //    success = true,
            //    response = LogLock.AccessApiByHour()
            //};

            return Success(LogLock.AccessApiByHour(), "获取成功");
        }

        private List<UserAccessModel> GetAccessLogsToday(IWebHostEnvironment environment)
        {
            List<UserAccessModel> userAccessModels = new();
            var accessLogs = LogLock.ReadLog(
                Path.Combine(environment.ContentRootPath, "Log"), "RecordAccessLogs_", Encoding.UTF8, ReadType.PrefixLatest
                ).ObjToString();
            try
            {
                return JsonConvert.DeserializeObject<List<UserAccessModel>>("[" + accessLogs + "]");
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
                            var accItem = JsonConvert.DeserializeObject<UserAccessModel>(item.TrimEnd(','));
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
        private List<ActiveUserVM> GetAccessLogsTrend(IWebHostEnvironment environment)
        {
            List<ActiveUserVM> userAccessModels = new();
            var accessLogs = LogLock.ReadLog(
                Path.Combine(environment.ContentRootPath, "Log"), "ACCESSTRENDLOG_", Encoding.UTF8, ReadType.PrefixLatest
                ).ObjToString();
            try
            {
                return JsonConvert.DeserializeObject<List<ActiveUserVM>>(accessLogs);
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
                            var accItem = JsonConvert.DeserializeObject<ActiveUserVM>(item.TrimStart('[').TrimEnd(']'));
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

        [HttpGet]
        public MessageModel<WelcomeInitData> GetActiveUsers([FromServices] IWebHostEnvironment environment)
        {
            var accessLogsToday = GetAccessLogsToday(environment).Where(d => d.BeginTime.ObjToDate() >= DateTime.Today);

            var Logs = accessLogsToday.OrderByDescending(d => d.BeginTime).Take(50).ToList();

            var errorCountToday = LogLock.GetLogData().Where(d => d.Import == 9).Count();

            accessLogsToday = accessLogsToday.Where(d => d.User != "").ToList();

            var activeUsers = (from n in accessLogsToday
                               group n by new { n.User } into g
                               select new ActiveUserVM
                               {
                                   user = g.Key.User,
                                   count = g.Count(),
                               }).ToList();

            int activeUsersCount = activeUsers.Count;
            activeUsers = activeUsers.OrderByDescending(d => d.count).Take(10).ToList();

            //return new MessageModel<WelcomeInitData>()
            //{
            //    msg = "获取成功",
            //    success = true,
            //    response = new WelcomeInitData()
            //    {
            //        activeUsers = activeUsers,
            //        activeUserCount = activeUsersCount,
            //        errorCount = errorCountToday,
            //        logs = Logs,
            //        activeCount = GetAccessLogsTrend(environment)
            //    }
            //};

            return Success(new WelcomeInitData()
            {
                activeUsers = activeUsers,
                activeUserCount = activeUsersCount,
                errorCount = errorCountToday,
                logs = Logs,
                activeCount = GetAccessLogsTrend(environment)
            }, "获取成功");
        }

        [HttpGet]
        public async Task<MessageModel<AccessApiDateView>> GetIds4Users()
        {
            List<ApiDate> apiDates = new List<ApiDate>();

            if (Appsettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
            {
                var users = await _applicationUserServices.Query(d => d.tdIsDelete == false);

                apiDates = (from n in users
                            group n by new { n.birth.Date } into g
                            select new ApiDate
                            {
                                date = g.Key?.Date.ToString("yyyy-MM-dd"),
                                count = g.Count(),
                            }).ToList();

                apiDates = apiDates.OrderByDescending(d => d.date).Take(30).ToList();
            }


            if (apiDates.Count == 0)
            {
                apiDates.Add(new ApiDate()
                {
                    date = "没数据,或未开启相应接口服务",
                    count = 0
                });
            }
            //return new MessageModel<AccessApiDateView>()
            //{
            //    msg = "获取成功",
            //    success = true,
            //    response = new AccessApiDateView
            //    {
            //        columns = new string[] { "date", "count" },
            //        rows = apiDates.OrderBy(d => d.date).ToList(),
            //    }
            //};

            return Success(new AccessApiDateView
            {
                columns = new string[] { "date", "count" },
                rows = apiDates.OrderBy(d => d.date).ToList(),
            }, "获取成功");
        }

    }

    public class WelcomeInitData
    {
        public List<ActiveUserVM> activeUsers { get; set; }
        public int activeUserCount { get; set; }
        public List<UserAccessModel> logs { get; set; }
        public int errorCount { get; set; }
        public List<ActiveUserVM> activeCount { get; set; }
    }

}
