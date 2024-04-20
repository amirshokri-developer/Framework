using RabbitMQ.Client;

namespace ASh.Framework.EventBus.RabbitMQ.Abstractions
{
    internal interface IQueueChannelProvider<TIntegrationEvent>
    {
        IModel GetChannel();
    }
}
