using System;
using Blog.Core.Extensions.HostedService;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Core.Extensions;

public static class InitializationHostServiceSetup
{
    /// <summary>
    /// 应用初始化服务注入
    /// </summary>
    /// <param name="services"></param>
    public static void AddInitializationHostServiceSetup(this IServiceCollection services)
    {
        if (services is null)
        {
            ArgumentNullException.ThrowIfNull(nameof(services));
        }
        services.AddHostedService<SeedDataHostedService>();
        services.AddHostedService<QuartzJobHostedService>();
        services.AddHostedService<ConsulHostedService>();
        services.AddHostedService<EventBusHostedService>();
    }
}