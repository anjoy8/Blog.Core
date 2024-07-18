using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// IPLimit限流 启动服务
    /// </summary>
    public static class IpPolicyRateLimitSetup
    {
        public static void AddIpPolicyRateLimitSetup(this IServiceCollection services, IConfiguration Configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            // CacheSetup unified register
            // services.AddMemoryCache();
            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            // inject counter and rules stores
            // services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            // services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            // inject counter and rules distributed cache stores
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            // the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}