using Autofac;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Core.EventBus
{
    /// <summary>
    /// Kafka consumer 监听服务
    /// </summary>
    public class KafkaConsumerHostService : BackgroundService
    {
        private readonly string AUTOFAC_SCOPE_NAME = "blogcore_event_bus";
        private readonly ILogger<KafkaConsumerHostService> _logger;
        private readonly IConsumer<string, byte[]> _consumer;
        private readonly KafkaOptions _options;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private CancellationTokenSource cts = new();
        public KafkaConsumerHostService(ILogger<KafkaConsumerHostService> logger,
            IOptions<KafkaOptions> options,
            IEventBusSubscriptionsManager eventBusSubscriptionsManager,
            ILifetimeScope autofac)
        {
            _autofac = autofac;
            _subsManager = eventBusSubscriptionsManager;
            _logger = logger;
            _options = options.Value;
            _consumer = new ConsumerBuilder<string, byte[]>(new ConsumerConfig
            {
                BootstrapServers = _options.Servers,
                GroupId = _options.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true,
                EnableAutoCommit = false,
                LogConnectionClose = false
            }).SetErrorHandler(ConsumerClient_OnConsumeError)
            .Build();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var result =  await FetchTopicAsync();
            if (result)
            {
                _consumer.Subscribe(_options.Topic);
                while (!cts.Token.IsCancellationRequested)
                {
                    var consumerResult = _consumer.Consume(cts.Token);
                    try
                    {
                        if (consumerResult.IsPartitionEOF || consumerResult.Message.Value == null) continue;

                        var @event = Protobuf.Deserialize<string>(consumerResult.Message.Value);
                        await ProcessEvent(consumerResult.Message.Key, @event);
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error occured: {e.Error.Reason}");
                    }
                    finally
                    {
                        _consumer.Commit(consumerResult);
                    }
                }
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            cts.Cancel();
            _logger.LogInformation("kafka consumer stop and disposable");
            _consumer.Dispose();
            return base.StopAsync(cancellationToken);
        }
        /// <summary>
        /// 检测当前Topic是否存在
        /// </summary>
        /// <returns></returns>
        private async Task<bool> FetchTopicAsync()
        {
            if (string.IsNullOrEmpty(_options.Topic))
                throw new ArgumentNullException(nameof(_options.Topic));
            
            try
            {
                var config = new AdminClientConfig { BootstrapServers = _options.Servers };
                using var adminClient = new AdminClientBuilder(config).Build();
                await adminClient.CreateTopicsAsync(Enumerable.Range(0,1).Select(u=> new TopicSpecification
                {
                    Name = _options.Topic,
                    NumPartitions = _options.NumPartitions
                }));
            }
            catch (CreateTopicsException ex) when (ex.Message.Contains("already exists"))
            {
            }
            catch (Exception ex)
            {
                _logger.LogError("An error was encountered when automatically creating topic! -->" + ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 接收到消息进行处理
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing Kafka event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            if (handler == null) continue;
                            dynamic eventData = JObject.Parse(message);

                            await Task.Yield();
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            await Task.Yield();
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for Kafka event: {EventName}", eventName);
            }
        }

        private void ConsumerClient_OnConsumeError(IConsumer<string, byte[]> consumer, Error e)
        {
            _logger.LogError("An error occurred during connect kafka:" + e.Reason);
        }
    }
}
