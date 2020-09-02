using Blog.Core.Common;
using Blog.Core.Common.Redis;
using InitQ;
using InitQ.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Redis 消息队列 启动服务
    /// </summary>
    public static class RedisInitMqSetup
    {
        public static void AddRedisInitMqSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddInitQ(m =>
            {
                m.SuspendTime = 5000;
                m.ConnectionString = "127.0.0.1:6379";
                m.ListSubscribe = new List<IRedisSubscribe>() { new RedisSubscribe()};
                m.ShowLog = false;
            });
        }
    }
}
