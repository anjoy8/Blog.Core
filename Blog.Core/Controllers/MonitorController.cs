using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Common.Helper;
using Blog.Core.Common.LogHelper;
using Blog.Core.Hubs;
using Blog.Core.Model;
using Blog.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Core.Controllers
{
    [Route("api/[Controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class MonitorController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IWebHostEnvironment _env;

        public MonitorController(IHubContext<ChatHub> hubContext, IWebHostEnvironment env)
        {
            _hubContext = hubContext;
            _env = env;
        }

        /// <summary>
        /// 服务器配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageModel<ServerViewModel> Server()
        {
            return new MessageModel<ServerViewModel>()
            {
                msg = "获取成功",
                success = true,
                response = new ServerViewModel()
                {
                    EnvironmentName = _env.EnvironmentName,
                    OSArchitecture = RuntimeInformation.OSArchitecture.ObjToString(),
                    ContentRootPath = _env.ContentRootPath,
                    WebRootPath = _env.WebRootPath,
                    FrameworkDescription = RuntimeInformation.FrameworkDescription,
                    MemoryFootprint = (Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2") + " MB",
                    WorkingTime = DateHelper.TimeSubTract(DateTime.Now, Process.GetCurrentProcess().StartTime)
                }
            };
        }


        /// <summary>
        /// SignalR send data
        /// </summary>
        /// <returns></returns>
        // GET: api/Logs
        [HttpGet]
        public MessageModel<List<LogInfo>> Get()
        {

            _hubContext.Clients.All.SendAsync("ReceiveUpdate", LogLock.GetLogData()).Wait();

            return new MessageModel<List<LogInfo>>()
            {
                msg = "获取成功",
                success = true,
                response = null
            };
        }



        [HttpGet]
        public MessageModel<RequestApiWeekView> GetRequestApiinfoByWeek()
        {
            return new MessageModel<RequestApiWeekView>()
            {
                msg = "获取成功",
                success = true,
                response = LogLock.RequestApiinfoByWeek()
            };
        }

        [HttpGet]
        public MessageModel<AccessApiDateView> GetAccessApiByDate()
        {
            return new MessageModel<AccessApiDateView>()
            {
                msg = "获取成功",
                success = true,
                response = LogLock.AccessApiByDate()
            };
        }

        [HttpGet]
        public MessageModel<AccessApiDateView> GetAccessApiByHour()
        {
            return new MessageModel<AccessApiDateView>()
            {
                msg = "获取成功",
                success = true,
                response = LogLock.AccessApiByHour()
            };
        }

    }


}
