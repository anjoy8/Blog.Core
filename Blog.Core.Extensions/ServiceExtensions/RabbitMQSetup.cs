using Blog.Core.Common;
using Blog.Core.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// Db 启动服务
    /// </summary>
    public static class RabbitMQSetup
    {
        public static void AddRabbitMQSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettings.app(new string[] { "RabbitMQ", "Enabled" }).ObjToBool())
            {
                services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                   {
                       var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();

                       var factory = new ConnectionFactory()
                       {
                           HostName = AppSettings.app(new string[] { "RabbitMQ", "Connection" }),
                           DispatchConsumersAsync = true
                       };

                       if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "UserName" })))
                       {
                           factory.UserName = AppSettings.app(new string[] { "RabbitMQ", "UserName" });
                       }

                       if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "Password" })))
                       {
                           factory.Password = AppSettings.app(new string[] { "RabbitMQ", "Password" });
                       }

                       if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "Port" })))
                       {
                           factory.Port = AppSettings.app(new string[] { "RabbitMQ", "Port" }).ObjToInt();
                       }

                       var retryCount = 5;
                       if (!string.IsNullOrEmpty(AppSettings.app(new string[] { "RabbitMQ", "RetryCount" })))
                       {
                           retryCount = AppSettings.app(new string[] { "RabbitMQ", "RetryCount" }).ObjToInt();
                       }

                       return new RabbitMQPersistentConnection(factory, logger, retryCount);
                   });
            }
        }
    }
}
