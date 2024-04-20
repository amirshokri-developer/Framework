namespace ASh.Framework.EventBus.RabbitMQ.Abstractions
{
    internal interface IHandlerRegistrator<TIntegrationEventHandler, TIntegrationEvent>
    {
        void Register();
        void UnRegister();
    }
}
