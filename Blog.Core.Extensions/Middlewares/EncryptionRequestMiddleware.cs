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
    public class EncryptionRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public EncryptionRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 配置开关，过滤接口
            if (AppSettings.app("Middleware", "EncryptionRequest", "Enabled").ObjToBool())
            {
                var isAllApis = AppSettings.app("Middleware", "EncryptionRequest", "AllApis").ObjToBool();
                var needEnApis = AppSettings.app<string>("Middleware", "EncryptionRequest", "LimitApis");
                var path = context.Request.Path.Value.ToLower();
                if (isAllApis || (path.Length > 5 && needEnApis.Any(d => d.ToLower().Contains(path))))
                {
                    Console.WriteLine($"{isAllApis} -- {path}");

                    if (context.Request.Method.ToLower() == "post")
                    {
                        // 读取请求主体
                        using StreamReader reader = new(context.Request.Body, Encoding.UTF8);
                        string requestBody = await reader.ReadToEndAsync();

                        // 检查是否有要解密的数据
                        if (!string.IsNullOrEmpty(requestBody) && context.Request.Headers.ContainsKey("Content-Type") &&
                            context.Request.Headers["Content-Type"].ToString().ToLower().Contains("application/json"))
                        {
                            // 解密数据
                            string decryptedString = DecryptData(requestBody);

                            // 更新请求主体中的数据
                            context.Request.Body = GenerateStreamFromString(decryptedString);
                        }
                    }
                    else if (context.Request.Method.ToLower() == "get")
                    {
                        // 获取url参数
                        string param = context.Request.Query["param"];

                        // 检查是否有要解密的数据
                        if (!string.IsNullOrEmpty(param))
                        {
                            // 解密数据
                            string decryptedString = DecryptData(param);

                            // 更新url参数值
                            context.Request.QueryString = new QueryString($"?{decryptedString}");
                        }
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
        private string DecryptData(string encryptedData)
        {
            // 解密逻辑实现，可以根据你使用的加密算法和密钥进行自定义
            byte[] bytes = Convert.FromBase64String(encryptedData);
            string originalString = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(originalString);
            return originalString;
        }
        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

    public static class EncryptionRequestExtensions
    {
        /// <summary>
        /// 自定义中间件
        /// 通过配置，对指定接口入参进行解密操作
        /// 注意：放到管道最外层
        /// </summary>
        public static IApplicationBuilder UseEncryptionRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EncryptionRequestMiddleware>();
        }
    }
}
