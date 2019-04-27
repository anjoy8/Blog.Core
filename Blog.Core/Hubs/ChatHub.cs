using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Common.Helper;
using Blog.Core.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Core.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task GetLatestCount(string random)
        {
            int count = 0;
            //while (count < 10)
            {
                count++;

                await Clients.All.SendAsync("ReceiveUpdate", GetLogData());

                //Thread.Sleep(10000);
            }

        }


        private List<LogInfo> GetLogData() {
            var aopLogs = FileHelper.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "Log", "AOPLog.log"), Encoding.UTF8)
           .Split("--------------------------------")
           .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
           .Select(d => new LogInfo
           {
               Datetime = d.Split("|")[0],
               Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
               LogColor = "AOP",
           }).ToList();


            var excLogs = FileHelper.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "Log", $"GlobalExcepLogs_{System.DateTime.Now.ToString("yyyMMdd")}.log"), Encoding.UTF8)
                .Split("--------------------------------")
                .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                .Select(d => new LogInfo
                {
                    Datetime = d.Split("|")[0],
                    Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                    LogColor = "EXC",
                }).ToList();


            var sqlLogs = FileHelper.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), "Log", "SqlLog.log"), Encoding.UTF8)
                .Split("--------------------------------")
                .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                .Select(d => new LogInfo
                {
                    Datetime = d.Split("|")[0],
                    Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                    LogColor = "SQL",
                }).ToList();

            aopLogs.AddRange(excLogs);
            aopLogs.AddRange(sqlLogs);
            aopLogs = aopLogs.OrderByDescending(d => d.Datetime).Take(100).ToList();

            return aopLogs;
        }
    }
}
