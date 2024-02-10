using ASh.Framework.EventBus.Events;

namespace ASh.Framework.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent @event);
    }

   
}
