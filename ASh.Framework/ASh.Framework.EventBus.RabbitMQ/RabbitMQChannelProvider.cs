using ASh.Framework.EventBus.Events;
using ASh.Framework.EventBus.RabbitMQ.Abstractions;
using RabbitMQ.Client;

namespace ASh.Framework.EventBus.RabbitMQ
{
    internal class RabbitMQChannelProvider<TIntegrationEvent> :
        IQueueChannelProvider<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        private readonly IChannelProvider _channelProvider;
        private IModel _channel;
        private readonly string _queueName;
        private readonly string _exchangeName;
        public RabbitMQChannelProvider(IChannelProvider channelProvider)
        {
            _channelProvider = channelProvider;
            _queueName = typeof(TIntegrationEvent).Name;
            _exchangeName = typeof(TIntegrationEvent).Name;

        }

        public IModel GetChannel()
        {
            _channel = _channelProvider.GetChannel();
            DeclareQueueAndDeadLetter();
            return _channel;
        }

        private void DeclareQueueAndDeadLetter()
        {
            var deadLetterQueueName = $"{_queueName}_dlq";
            var deadLetterQueueExchangeName = $"{_exchangeName}_dlq";

            // Declare the DeadLetter Queue
            var deadLetterQueueArgs = new Dictionary<string, object>
        {
            { "x-queue-type", "quorum" },
            { "overflow", "reject-publish" } // If the queue is full, reject the publish
        };

            _channel.ExchangeDeclare(deadLetterQueueExchangeName, ExchangeType.Fanout);
            _channel.QueueDeclare(deadLetterQueueName, true, false, false, deadLetterQueueArgs);
            _channel.QueueBind(deadLetterQueueName, deadLetterQueueExchangeName, deadLetterQueueName, null);

            // Declare the Queue
            var queueArgs = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", deadLetterQueueExchangeName },
            { "x-dead-letter-routing-key", deadLetterQueueName },
            { "x-queue-type", "quorum" },
            { "x-dead-letter-strategy", "at-least-once" }, // Ensure that deadletter messages are delivered in any case see: https://www.rabbitmq.com/quorum-queues.html#dead-lettering
            { "overflow", "reject-publish" } // If the queue is full, reject the publish
        };

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout);
            _channel.QueueDeclare(_queueName, true, false, false, queueArgs);
            _channel.QueueBind(_queueName, _exchangeName, _queueName, null);
        }
    }
}
