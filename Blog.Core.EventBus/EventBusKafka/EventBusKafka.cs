using Blog.Core.Common.Extensions;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace Blog.Core.EventBus
{
    /// <summary>
    /// 基于Kafka的事件总线
    /// </summary>
    public class EventBusKafka : IEventBus
    {
        private readonly ILogger<EventBusKafka> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IKafkaConnectionPool _connectionPool;
        private readonly KafkaOptions _options;
        public EventBusKafka(ILogger<EventBusKafka> logger,
            IEventBusSubscriptionsManager subsManager,
            IKafkaConnectionPool connectionPool,
            IOptions<KafkaOptions> options)
        {
            _logger = logger;
            _subsManager = subsManager;
            _connectionPool = connectionPool;
            _options = options.Value;
        }
        /// <summary>
        /// 发布
        /// </summary>
        public void Publish(IntegrationEvent @event)
        {
            var producer =  _connectionPool.Producer();
            try
            {
                var eventName = @event.GetType().Name;
                var body = Protobuf.Serialize(JsonConvert.SerializeObject(@event));
                DeliveryResult<string, byte[]> result = producer.ProduceAsync(_options.Topic, new Message<string, byte[]>
                {
                    Key = eventName,
                    Value = body
                }).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Could not publish event: {@event.Id.ToString("N")} ({ex.Message});  Message:{ JsonConvert.SerializeObject(@event)}");
            }
            finally
            {
                //放入连接池中
                _connectionPool.Return(producer);
            }
        }

        /// <summary>
        /// 订阅
        /// 动态
        /// </summary>
        /// <typeparam name="TH">事件处理器</typeparam>
        /// <param name="eventName">事件名</param>
        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T">约束：事件模型</typeparam>
        /// <typeparam name="TH">约束：事件处理器<事件模型></typeparam>
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            _subsManager.AddSubscription<T, TH>();
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            if (_connectionPool != null)
            {
                _connectionPool.Dispose();
            }
            _subsManager.Clear();
        }

    }
}
