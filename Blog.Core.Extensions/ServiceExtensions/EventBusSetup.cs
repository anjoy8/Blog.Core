using Autofac;
using Blog.Core.Common;
using Blog.Core.EventBus;
using Blog.Core.EventBus.EventHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// EventBus 事件总线服务
    /// </summary>
    public static class EventBusSetup
    {
        public static void AddEventBusSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (Appsettings.app(new string[] { "RabbitMQ", "Enabled" }).ObjToBool() && Appsettings.app(new string[] { "EventBus", "Enabled" }).ObjToBool())
            {
                var subscriptionClientName = Appsettings.app(new string[] { "EventBus", "SubscriptionClientName" });


                services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
                services.AddTransient<BlogDeletedIntegrationEventHandler>();


                services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                {
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(Appsettings.app(new string[] { "RabbitMQ", "RetryCount" })))
                    {
                        retryCount = int.Parse(Appsettings.app(new string[] { "RabbitMQ", "RetryCount" }));
                    }

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
                });
            }
        }


        public static void ConfigureEventBus(this IApplicationBuilder app)
        {
            if (Appsettings.app(new string[] { "RabbitMQ", "Enabled" }).ObjToBool() && Appsettings.app(new string[] { "EventBus", "Enabled" }).ObjToBool())
            {
                var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

                eventBus.Subscribe<BlogDeletedIntegrationEvent, BlogDeletedIntegrationEventHandler>(); 
            }
        }
    }
}
