using Blog.Core.Common;
using Blog.Core.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// 自定义中间件
    /// 通过配置，对指定接口返回数据进行加密返回
    /// 可过滤文件流
    /// </summary>
    public class EncryptionResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public EncryptionResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 配置开关，过滤接口
            if (AppSettings.app("Middleware", "EncryptionResponse", "Enabled").ObjToBool())
            {
                var isAllApis = AppSettings.app("Middleware", "EncryptionResponse", "AllApis").ObjToBool();
                var needEnApis = AppSettings.app<string>("Middleware", "EncryptionResponse", "LimitApis");
                var path = context.Request.Path.Value.ToLower();
                if (isAllApis || (path.Length > 5 && needEnApis.Any(d => d.ToLower().Contains(path))))
                {
                    Console.WriteLine($"{isAllApis} -- {path}");
                    var responseCxt = context.Response;
                    var originalBodyStream = responseCxt.Body;

                    // 创建一个新的内存流用于存储加密后的数据
                    using var encryptedBodyStream = new MemoryStream();
                    // 用新的内存流替换 responseCxt.Body
                    responseCxt.Body = encryptedBodyStream;

                    // 执行下一个中间件请求管道
                    await _next(context);

                    //encryptedBodyStream.Seek(0, SeekOrigin.Begin);
                    //encryptedBodyStream.Position = 0;

                    // 可以去掉某些流接口
                    if (!context.Response.ContentType.ToLower().Contains("application/json"))
                    {
                        Console.WriteLine($"非json返回格式 {context.Response.ContentType}");
                        //await encryptedBodyStream.CopyToAsync(originalBodyStream);
                        context.Response.Body = originalBodyStream;
                        return;
                    }

                    // 读取加密后的数据
                    //var encryptedBody = await new StreamReader(encryptedBodyStream).ReadToEndAsync();
                    var encryptedBody = responseCxt.GetResponseBody();

                    if (encryptedBody.IsNotEmptyOrNull())
                    {
                        dynamic jsonObject = JsonConvert.DeserializeObject(encryptedBody);
                        string statusCont = jsonObject.status;
                        var status = statusCont.ObjToInt();
                        string msg = jsonObject.msg;
                        string successCont = jsonObject.success;
                        var success = successCont.ObjToBool();
                        dynamic responseCnt = success ? jsonObject.response : "";
                        string s = "1";
                        // 这里换成自己的任意加密方式
                        var response = responseCnt.ToString() != "" ? Convert.ToBase64String(Encoding.UTF8.GetBytes(responseCnt.ToString())) : "";
                        string resJson = JsonConvert.SerializeObject(new { response, msg, status, s, success });

                        context.Response.Clear();
                        responseCxt.ContentType = "application/json";

                        //await using var streamlriter = new StreamWriter(originalBodyStream, leaveOpen: true);
                        //await streamlriter.WriteAsync(resJson);

                        var encryptedData = Encoding.UTF8.GetBytes(resJson);
                        responseCxt.ContentLength = encryptedData.Length;
                        await originalBodyStream.WriteAsync(encryptedData, 0, encryptedData.Length);

                        responseCxt.Body = originalBodyStream;
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
    }

    public static class EncryptionResponseExtensions
    {
        /// <summary>
        /// 自定义中间件
        /// 通过配置，对指定接口返回数据进行加密返回
        /// 可过滤文件流
        /// 注意：放到管道最外层
        /// </summary>
        public static IApplicationBuilder UseEncryptionResponse(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EncryptionResponseMiddleware>();
        }
    }
}
