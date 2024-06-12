using Blog.Core.Common;
using Blog.Core.Common.Option;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Blog.Core.Extensions.ServiceExtensions;

public static class DataProtectionSetup
{
    public static void AddDataProtectionSetup(this IServiceCollection services)
    {
        var builder = services.AddDataProtection();

        var redisOption = App.GetOptions<RedisOptions>();
        if (redisOption.Enable)
        {
            builder.PersistKeysToStackExchangeRedis(App.GetService<IConnectionMultiplexer>());
        }
    }
}