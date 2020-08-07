using Blog.Core.Common;
using Blog.Core.Common.HttpContextUser;
using Blog.Core.Common.LogHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Blog.Core.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录用户方访问数据
    /// </summary>
    public class RecordAccessLogsMildd
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly IUser _user;
        private readonly ILogger<RecordAccessLogsMildd> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public RecordAccessLogsMildd(RequestDelegate next, IUser user, ILogger<RecordAccessLogsMildd> logger)
        {
            _next = next;
            _user = user;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("Middleware", "RecordAccessLogs", "Enabled").ObjToBool())
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.Contains("api"))
                {

                    //记录Job时间
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    context.Request.EnableBuffering();
                    Stream originalBody = context.Response.Body;

                    try
                    {

                        using (var ms = new MemoryStream())
                        {
                            context.Response.Body = ms;

                            stopwatch.Stop();
                            var opTime = stopwatch.Elapsed.TotalMilliseconds.ToString("00") + "ms";
                            // 存储请求数据
                            await RequestDataLog(context, opTime);

                            await _next(context);

                            ms.Position = 0;
                            await ms.CopyToAsync(originalBody);
                        }

                    }
                    catch (Exception ex)
                    {
                        // 记录异常
                        _logger.LogError(ex.Message + "\r\n" + ex.InnerException);
                    }
                    finally
                    {
                        context.Response.Body = originalBody;
                    }
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

        private async Task RequestDataLog(HttpContext context, string opTime)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var requestData = request.Method == "GET" || request.Method == "DELETE" ? HttpUtility.UrlDecode(request.QueryString.ObjToString(), Encoding.UTF8) : (await sr.ReadToEndAsync()).ObjToString();
            if (requestData.IsNotEmptyOrNull() && requestData.Length > 30)
            {
                requestData = requestData.Substring(0, 30);
            }

            var requestInfo = JsonConvert.SerializeObject(new UserAccessModel()
            {
                User = _user.Name,
                IP = IPLogMildd.GetClientIP(context),
                API = request.Path.ObjToString().TrimEnd('/').ToLower(),
                BeginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                OPTime = opTime,
                RequestMethod = request.Method,
                RequestData = requestData,
                Agent = request.Headers["User-Agent"].ObjToString()
            });

            if (!string.IsNullOrEmpty(requestInfo))
            {
                // 自定义log输出
                Parallel.For(0, 1, e =>
                {
                    LogLock.OutSql2Log("RecordAccessLogs", new string[] { requestInfo + "," }, false);
                });

                request.Body.Position = 0;
            }
        }
    }

    public class UserAccessModel
    {
        public string User { get; set; }
        public string IP { get; set; }
        public string API { get; set; }
        public string BeginTime { get; set; }
        public string OPTime { get; set; }
        public string RequestMethod { get; set; }
        public string RequestData { get; set; }
        public string Agent { get; set; }

    }

}

