using Blog.Core.Common;
using Blog.Core.Common.LogHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Extensions.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录IP请求数据
    /// </summary>
    public class IpLogMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;

        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<IpLogMiddleware> _logger;

        public IpLogMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILogger<IpLogMiddleware> logger)
        {
            _next        = next;
            _environment = environment;
            _logger      = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (AppSettings.app("Middleware", "IPLog", "Enabled").ObjToBool())
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.Contains("api"))
                {
                    context.Request.EnableBuffering();


                    // 存储请求数据
                    var request = context.Request;

                    var requestInfo = JsonConvert.SerializeObject(new RequestInfo()
                    {
                        Ip       = GetClientIP(context),
                        Url      = request.Path.ObjToString().TrimEnd('/').ToLower(),
                        Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Date     = DateTime.Now.ToString("yyyy-MM-dd"),
                        Week     = GetWeek(),
                    });

                    if (!string.IsNullOrEmpty(requestInfo))
                    {
                        // 自定义log输出
                        _logger.LogInformation("RequestIpInfoLog:{TraceIdentifier}: {RequestInfo}",
                            context.TraceIdentifier,
                            requestInfo);
                        
                        //try
                        //{
                        //    var testLogMatchRequestInfo = JsonConvert.DeserializeObject<RequestInfo>(requestInfo);
                        //    if (testLogMatchRequestInfo != null)
                        //    {
                        //        var logFileName = FileHelper.GetAvailableFileNameWithPrefixOrderSize(_environment.ContentRootPath, "RequestIpInfoLog");
                        //        SerilogServer.WriteLog(logFileName, new string[] { requestInfo + "," }, false, "", true);
                        //    }
                        //}
                        //catch (Exception e)
                        //{
                        //    log.Error(requestInfo + "\r\n" + e.GetBaseException().ToString());
                        //}

                        request.Body.Position = 0;
                    }

                    await _next(context);
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private string GetWeek()
        {
            string week = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "周三";
                    break;
                case DayOfWeek.Thursday:
                    week = "周四";
                    break;
                case DayOfWeek.Friday:
                    week = "周五";
                    break;
                case DayOfWeek.Saturday:
                    week = "周六";
                    break;
                case DayOfWeek.Sunday:
                    week = "周日";
                    break;
                default:
                    week = "N/A";
                    break;
            }

            return week;
        }

        public static string GetClientIP(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].ObjToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ObjToString();
            }

            return ip;
        }
    }
}