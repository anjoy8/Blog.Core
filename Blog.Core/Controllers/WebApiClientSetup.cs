using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiClient.Extensions.DependencyInjection;
using Blog.Core.Common.WebApiClients;

namespace Blog.Core.Extensions
{
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

            services.AddHttpApi<IValuesApi>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri("http://apk.neters.club/");
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            });
        }
    }
}