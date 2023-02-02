using Microsoft.AspNetCore.Builder;

namespace Blog.Core.Extensions.Middlewares
{
    public static class MiddlewareHelpers
    {
        /// <summary>
        /// 自定义授权中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJwtTokenAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtTokenAuthMiddleware>();
        }

        /// <summary>
        /// 请求响应中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestResponseLogMiddle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequRespLogMiddleware>();
        }

        /// <summary>
        /// SignalR中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSignalRSendMiddle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SignalRSendMiddleware>();
        }

        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandlerMiddle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        /// <summary>
        /// IP请求中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIpLogMiddle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<IpLogMiddleware>();
        }

        /// <summary>
        /// 用户访问中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRecordAccessLogsMiddle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RecordAccessLogsMiddleware>();
        }
    }
}
