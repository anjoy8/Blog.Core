using System;
using System.Net;
using System.Threading.Tasks;
using Blog.Core.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Blog.Core.Extensions.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            if (e == null) return;

            await WriteExceptionAsync(context, e).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception e)
        {
            var message = e.Message;
            switch (e)
            {
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            await context.Response
                .WriteAsync(JsonConvert.SerializeObject(new ApiResponse(StatusCode.CODE500, message).MessageModel))
                .ConfigureAwait(false);
        }
    }
}