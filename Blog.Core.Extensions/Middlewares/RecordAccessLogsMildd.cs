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
        private Stopwatch _stopwatch;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public RecordAccessLogsMildd(RequestDelegate next, IUser user, ILogger<RecordAccessLogsMildd> logger)
        {
            _next = next;
            _user = user;
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (Appsettings.app("Middleware", "RecordAccessLogs", "Enabled").ObjToBool())
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.Contains("api"))
                {
                    _stopwatch.Restart();
                    var userAccessModel = new UserAccessModel();

                    HttpRequest request = context.Request;

                    userAccessModel.API = request.Path.ObjToString().TrimEnd('/').ToLower();
                    userAccessModel.User = _user.Name;
                    userAccessModel.IP = IPLogMildd.GetClientIP(context);
                    userAccessModel.BeginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    userAccessModel.RequestMethod = request.Method;
                    userAccessModel.Agent = request.Headers["User-Agent"].ObjToString();


                    // 获取请求body内容
                    if (request.Method.ToLower().Equals("post") || request.Method.ToLower().Equals("put"))
                    {
                        // 启用倒带功能，就可以让 Request.Body 可以再次读取
                        request.EnableBuffering();

                        Stream stream = request.Body;
                        byte[] buffer = new byte[request.ContentLength.Value];
                        stream.Read(buffer, 0, buffer.Length);
                        userAccessModel.RequestData = Encoding.UTF8.GetString(buffer);

                        request.Body.Position = 0;
                    }
                    else if (request.Method.ToLower().Equals("get") || request.Method.ToLower().Equals("delete"))
                    {
                        userAccessModel.RequestData = HttpUtility.UrlDecode(request.QueryString.ObjToString(), Encoding.UTF8);
                    }

                    // 获取Response.Body内容
                    var originalBodyStream = context.Response.Body;
                    using (var responseBody = new MemoryStream())
                    {
                        context.Response.Body = responseBody;

                        await _next(context);

                        var responseBodyData = await GetResponse(context.Response);

                        await responseBody.CopyToAsync(originalBodyStream);
                    }

                    // 响应完成记录时间和存入日志
                    context.Response.OnCompleted(() =>
                    {
                        _stopwatch.Stop();

                        userAccessModel.OPTime = _stopwatch.ElapsedMilliseconds + "ms";

                        // 自定义log输出
                        var requestInfo = JsonConvert.SerializeObject(userAccessModel);
                        Parallel.For(0, 1, e =>
                        {
                            LogLock.OutSql2Log("RecordAccessLogs", new string[] { requestInfo + "," }, false);
                        });

                        return Task.CompletedTask;
                    });

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


        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
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

