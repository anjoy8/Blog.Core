using Blog.Core.Common;
using Blog.Core.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// 注入Kafka相关配置
    /// </summary>
   public static class KafkaSetup
    {
        public static void AddKafkaSetup(this IServiceCollection services,IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (Appsettings.app(new string[] { "Kafka", "Enabled" }).ObjToBool())
            {
                services.Configure<KafkaOptions>(configuration.GetSection("kafka"));
                services.AddSingleton<IKafkaConnectionPool,KafkaConnectionPool>();
            }
        }
    }
}
