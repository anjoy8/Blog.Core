using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Common.Helper;
using Blog.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    //[Authorize(PermissionNames.Permission)]
    [Route("api/[Controller]/[action]")]
    [ApiController]
    public class MonitorController : Controller
    {


        // GET: api/Logs
        [HttpGet]
        public async Task<MessageModel<List<LogInfo>>> Get()
        {
            var aopLogs = FileHelper.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "Log", "AOPLog.log"), Encoding.UTF8)
                .Split("--------------------------------")
                .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                .Select(d => new LogInfo
                {
                    Datetime = d.Split("|")[0],
                    Content = d.Split("|")[1],
                    LogColor="AOP",
                }).ToList();


            var excLogs = FileHelper.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "Log", $"GlobalExcepLogs_{System.DateTime.Now.ToString("yyyMMdd")}.log"), Encoding.UTF8)
                .Split("--------------------------------")
                .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                .Select(d => new LogInfo
                {
                    Datetime = d.Split("|")[0],
                    Content = d.Split("|")[1],
                    LogColor="EXC",
                }).ToList();


            var sqlLogs = FileHelper.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "Log", "SqlLog.log"), Encoding.UTF8)
                .Split("--------------------------------")
                .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                .Select(d => new LogInfo
                {
                    Datetime = d.Split("|")[0],
                    Content = d.Split("|")[1],
                    LogColor="SQL",
                }).ToList();

            aopLogs.AddRange(excLogs);
            aopLogs.AddRange(sqlLogs);

            return new MessageModel<List<LogInfo>>()
            {
                msg = "获取成功",
                success = true,
                response = aopLogs
            };
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Logs
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Logs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }

    public class LogInfo
    {
        public string Datetime { get; set; }
        public string Content { get; set; }
        public string IP { get; set; }
        public string LogColor { get; set; }
    }
}
