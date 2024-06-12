using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blog.Core.Common;
using Blog.Core.Common.Extensions;
using Blog.Core.Common.LogHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Blog.Core.Extensions.Middlewares
{
	/// <summary>
	/// 中间件
	/// 记录请求和响应数据
	/// </summary>
	public class RequRespLogMiddleware
	{
		/// <summary>
		/// 
		/// </summary>
		private readonly RequestDelegate _next;

		private readonly ILogger<RequRespLogMiddleware> _logger;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="next"></param>
		public RequRespLogMiddleware(RequestDelegate next, ILogger<RequRespLogMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}


		public async Task InvokeAsync(HttpContext context)
		{
			if (AppSettings.app("Middleware", "RequestResponseLog", "Enabled").ObjToBool())
			{
				// 过滤，只有接口
				if (context.Request.Path.Value.Contains("api"))
				{
					context.Request.EnableBuffering();

					// 存储请求数据
					await RequestDataLog(context);
					
					await _next(context);

					// 存储响应数据
					ResponseDataLog(context.Response);
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

		private async Task RequestDataLog(HttpContext context)
		{
			var request = context.Request;
			var sr = new StreamReader(request.Body);
			RequestLogInfo requestResponse = new RequestLogInfo()
			{
				Path = request.Path,
				QueryString = request.QueryString.ToString(),
				BodyData = await sr.ReadToEndAsync()
			};
			var content = JsonConvert.SerializeObject(requestResponse);
			//var content = $" QueryData:{request.Path + request.QueryString}\r\n BodyData:{await sr.ReadToEndAsync()}";

			if (!string.IsNullOrEmpty(content))
			{
				Parallel.For(0, 1, e =>
				{
					//LogLock.OutSql2Log("RequestResponseLog", new string[] { "Request Data:", content });
					LogLock.OutLogAOP("RequestResponseLog", context.TraceIdentifier,
						new string[] {"Request Data -  RequestJsonDataType:" + requestResponse.GetType().ToString(), content});
				});
				//SerilogServer.WriteLog("RequestResponseLog", new string[] { "Request Data:", content });

				request.Body.Position = 0;
			}
		}

		private void ResponseDataLog(HttpResponse response)
		{
			var responseBody = response.GetResponseBody();

			// 去除 Html
			var reg = "<[^>]+>";

			if (!string.IsNullOrEmpty(responseBody))
			{
				var isHtml = Regex.IsMatch(responseBody, reg);
                Parallel.For(0, 1, e =>
				{
					//LogLock.OutSql2Log("RequestResponseLog", new string[] { "Response Data:", ResponseBody });
					LogLock.OutLogAOP("RequestResponseLog", response.HttpContext.TraceIdentifier,
						new string[] {"Response Data -  ResponseJsonDataType:" + responseBody.GetType().ToString(), responseBody});
				});
				//SerilogServer.WriteLog("RequestResponseLog", new string[] { "Response Data:", responseBody });
			}
		}

		private void ResponseDataLog(HttpResponse response, MemoryStream ms)
		{
			ms.Position = 0;
			var responseBody = new StreamReader(ms).ReadToEnd();

			// 去除 Html
			var reg = "<[^>]+>";
			var isHtml = Regex.IsMatch(responseBody, reg);

			if (!string.IsNullOrEmpty(responseBody))
			{
				Parallel.For(0, 1, e =>
				{
					//LogLock.OutSql2Log("RequestResponseLog", new string[] { "Response Data:", ResponseBody });
					LogLock.OutLogAOP("RequestResponseLog", response.HttpContext.TraceIdentifier,
						new string[] {"Response Data -  ResponseJsonDataType:" + responseBody.GetType().ToString(), responseBody});
				});
				//SerilogServer.WriteLog("RequestResponseLog", new string[] { "Response Data:", responseBody });
			}
		}
	}
}