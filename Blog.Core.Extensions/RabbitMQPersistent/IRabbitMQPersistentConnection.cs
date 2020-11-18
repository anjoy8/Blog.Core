using RabbitMQ.Client;
using System;

namespace Blog.Core.Extensions.RabbitMQPersistent
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
