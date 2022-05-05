using Blog.Core.Common.HttpPolly;
using Blog.Core.Model;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net.Http;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class HttpPollySetup
    {
        public static void AddHttpPollySetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            #region Polly策略
            var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>() // 若超时则抛出此异常
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            });

            // 为每个重试定义超时策略
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);
            #endregion

            services.AddHttpClient(HttpEnum.Common.ToString(), c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(retryPolicy)
            // 将超时策略放在重试策略之内，每次重试会应用此超时策略
            .AddPolicyHandler(timeoutPolicy);

            services.AddHttpClient(HttpEnum.LocalHost.ToString(), c =>
            {
                c.BaseAddress = new Uri("http://www.localhost.com");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(retryPolicy)
            // 将超时策略放在重试策略之内，每次重试会应用此超时策略
            .AddPolicyHandler(timeoutPolicy);

            services.AddSingleton<IHttpPollyHelper, HttpPollyHelper>();
        }
    }
}
