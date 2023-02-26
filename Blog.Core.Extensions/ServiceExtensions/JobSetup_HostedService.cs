using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// 任务调度 启动服务
    /// </summary>
    public static class JobSetup_HostedService
    {
        public static void AddJobSetup_HostedService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddHostedService<Job1TimedService>();
            services.AddHostedService<Job2TimedService>();

        }
    }
}
