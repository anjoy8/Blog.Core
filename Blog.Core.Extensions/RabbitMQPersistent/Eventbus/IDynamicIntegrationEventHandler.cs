using System.Threading.Tasks;

namespace Blog.Core.Extensions.RabbitMQPersistent
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
