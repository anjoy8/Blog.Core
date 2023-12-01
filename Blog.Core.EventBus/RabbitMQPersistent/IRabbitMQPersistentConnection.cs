using RabbitMQ.Client;
using System;

namespace Blog.Core.EventBus
{
    /// <summary>
    /// RabbitMQ持久连接
    /// 接口
    /// </summary>
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        /// <summary>
        /// 是否已经连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 尝试重连
        /// </summary>
        /// <returns></returns>
        bool TryConnect();

        /// <summary>
        /// 创建Model
        /// </summary>
        /// <returns></returns>
        IModel CreateModel();

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        void PublishMessage(string message, string exchangeName, string routingKey);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="queueName"></param>
        void StartConsuming(string queueName);
    }
}
