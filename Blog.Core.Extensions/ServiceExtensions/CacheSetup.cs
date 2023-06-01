using Blog.Core.Common;
using Blog.Core.Common.Caches;
using Blog.Core.Common.Option;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Blog.Core.Extensions.ServiceExtensions;

public static class CacheSetup
{
    /// <summary>
    /// 统一注册缓存
    /// </summary>
    /// <param name="services"></param>
    public static void AddCacheSetup(this IServiceCollection services)
    {
        var cacheOptions = App.GetOptions<RedisOptions>();
        if (cacheOptions.Enable)
        {
            //使用Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheOptions.ConnectionString;
                if (!cacheOptions.InstanceName.IsNullOrEmpty()) options.InstanceName = cacheOptions.InstanceName;
            });
            
            services.AddTransient<IRedisBasketRepository, RedisBasketRepository>();
            // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                //获取连接字符串
                var configuration = ConfigurationOptions.Parse(cacheOptions.ConnectionString, true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });
        }
        else
        {
            //使用内存
            services.AddDistributedMemoryCache(); 
        }
        
        services.AddSingleton<ICaching, Caching>();
    }
}