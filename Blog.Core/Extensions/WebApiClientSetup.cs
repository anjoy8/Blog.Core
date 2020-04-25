using Blog.Core.Common.WebApiClients.HttpApis;
using WebApiClient.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// WebApiClientSetup 启动服务
    /// </summary>
    public static class WebApiClientSetup
    {
        /// <summary>
        /// 注册WebApiClient接口
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpApi(this IServiceCollection services)
        {
            services.AddHttpApi<IBlogApi>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri("http://apk.neters.club/");
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            });
        }
    }
}
