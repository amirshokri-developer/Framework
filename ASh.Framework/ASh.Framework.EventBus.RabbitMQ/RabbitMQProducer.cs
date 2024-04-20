using ASh.Framework.EventBus.Abstractions;
using ASh.Framework.EventBus.Events;
using ASh.Framework.EventBus.RabbitMQ.Abstractions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ASh.Framework.EventBus.RabbitMQ
{
    internal class RabbitMQProducer<TIntegrationEvent> :
        IProducer<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        private readonly ILogger<RabbitMQProducer<TIntegrationEvent>> _logger;
        private readonly string _queueName;
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly IModel _channel;


        public RabbitMQProducer(IQueueChannelProvider<TIntegrationEvent> channelProvider, 
            ILogger<RabbitMQProducer<TIntegrationEvent>> logger)
        {
            _logger = logger;
            _channel = channelProvider.GetChannel();
            _queueName = typeof(TIntegrationEvent).Name;
            _exchangeName = typeof(TIntegrationEvent).Name;
            _routingKey = typeof(TIntegrationEvent).Name;
            
        }

        public void Produce(TIntegrationEvent @event)
        {
            try
            {
                var serializedMessage = SerializeMessage(@event);

                var properties = _channel.CreateBasicProperties();

                properties.Persistent = true;
                
                _channel.BasicPublish(_exchangeName, _routingKey, properties, serializedMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private static byte[] SerializeMessage(TIntegrationEvent message)
        {
            var stringContent = JsonSerializer.Serialize(message);
            return Encoding.UTF8.GetBytes(stringContent);
        }
    }
}
