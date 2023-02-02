using Confluent.Kafka;
using System;


namespace Blog.Core.EventBus
{
    /// <summary>
    /// Kafka连接池
    /// </summary>
   public interface IKafkaConnectionPool:IDisposable
    {
        /// <summary>
        /// 取对象
        /// </summary>
        /// <returns></returns>
        IProducer<string, byte[]> Producer();

        /// <summary>
        /// 将对象放入连接池
        /// </summary>
        /// <param name="producer"></param>
        /// <returns></returns>
        bool Return(IProducer<string, byte[]> producer);
    }
}
