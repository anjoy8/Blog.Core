using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Common.Helper;
using Blog.Core.Common.LogHelper;
using Blog.Core.Hubs;
using Blog.Core.Model;
using Microsoft.AspNetCore.Authorization;
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

        public MonitorController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
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
