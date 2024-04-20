using RabbitMQ.Client;

namespace ASh.Framework.EventBus.RabbitMQ.Abstractions
{
    internal interface IConnectionProvider
    {
        IConnection GetConnection();
    }
}
