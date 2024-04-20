using ASh.Framework.EventBus.Events;

namespace ASh.Framework.EventBus.Abstractions
{
    public interface IProducer<in TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        void Produce(TIntegrationEvent @event);
    }
}
