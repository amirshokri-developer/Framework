using ASh.Framework.EventBus.Abstractions;
using ASh.Framework.EventBus.Events;
using ASh.Framework.EventBus.RabbitMQ.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ASh.Framework.EventBus.RabbitMQ
{
    internal class HandlerRegisterator<TIntegrationEventHandler, TIntegrationEvent> : 
        IHandlerRegistrator<TIntegrationEventHandler, TIntegrationEvent>
        where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HandlerRegisterator<TIntegrationEventHandler, TIntegrationEvent>> _logger;
        private readonly string _queueName;
        private IModel _consumerRegistrationChannel;
        private string _consumerTag;
        private readonly string _consumerName;

        public HandlerRegisterator(IServiceProvider serviceProvider,
            ILogger<HandlerRegisterator<TIntegrationEventHandler, TIntegrationEvent>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _queueName = typeof(TIntegrationEvent).Name;
            _consumerName = typeof(TIntegrationEventHandler).Name;
        }

        public void Register()
        {
            var scope = _serviceProvider.CreateScope();

            
            _consumerRegistrationChannel = 
                scope.ServiceProvider.GetRequiredService<IQueueChannelProvider<TIntegrationEvent>>().GetChannel();

            var consumer = new AsyncEventingBasicConsumer(_consumerRegistrationChannel);

            consumer.Received += OnMessageReceived;
            try
            {
                _consumerTag = _consumerRegistrationChannel.BasicConsume(_queueName, false, consumer);
            }
            catch (Exception ex)
            {
                var exMsg = $"BasicConsume failed for Queue '{_queueName}'";
                _logger.LogError(ex, exMsg);
                throw new Exception(exMsg);
            }
        }

        public void UnRegister()
        {
            try
            {
                _consumerRegistrationChannel.BasicCancel(_consumerTag);
            }
            catch (Exception ex)
            {
                var message = $"Error canceling QueueConsumer registration for {_consumerName}";
                _logger.LogError(message, ex);
                throw new Exception(message, ex);
            }
        }

        private async Task OnMessageReceived(object ch, BasicDeliverEventArgs ea)
        {
            
            var consumerScope = _serviceProvider.CreateScope();

            var consumingChannel = ((AsyncEventingBasicConsumer)ch).Model;
       
            try
            {
           
                var message = DeserializeMessage(ea.Body.ToArray());

                
                var handlerInstance = consumerScope.ServiceProvider.GetRequiredService<TIntegrationEventHandler>();


                await handlerInstance.HandleAsync(message);

               
                if (consumingChannel.IsClosed)
                {
                    throw new Exception("A channel is closed during processing");
                }
           
                consumingChannel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                var msg = $"Cannot handle consumption of a {_queueName} by {_consumerName}'";
                _logger.LogError(ex, msg);
                RejectMessage(ea.DeliveryTag, consumingChannel);
            }
            finally
            {
                consumerScope.Dispose();
            }
        }

        private void RejectMessage(ulong deliveryTag, IModel consumeChannel)
        {
            try
            {               
                consumeChannel.BasicReject(deliveryTag, false);
            }
            catch (Exception bex)
            {
                var bexMsg =
                    $"BasicReject failed";
                _logger.LogCritical(bex, bexMsg);
            }
        }

        private static TIntegrationEvent DeserializeMessage(byte[] message)
        {
            var stringMessage = Encoding.UTF8.GetString(message);
            return JsonSerializer.Deserialize<TIntegrationEvent>(stringMessage);
        }
    }
}
