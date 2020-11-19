using System.Threading.Tasks;

namespace Blog.Core.EventBus
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
