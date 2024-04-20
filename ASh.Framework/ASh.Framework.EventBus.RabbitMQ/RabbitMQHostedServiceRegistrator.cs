using ASh.Framework.EventBus.Abstractions;
using ASh.Framework.EventBus.Events;
using ASh.Framework.EventBus.RabbitMQ.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASh.Framework.EventBus.RabbitMQ
{
    internal class RabbitMQHostedServiceRegistrator<TIntegrationEventHandler, TIntegrationEvent> :
        IHostedService where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent :  IntegrationEvent
    {
        private readonly ILogger<RabbitMQHostedServiceRegistrator<TIntegrationEventHandler, TIntegrationEvent>> _logger;
        private IHandlerRegistrator<TIntegrationEventHandler, TIntegrationEvent> _consumerHandler;
        private readonly IServiceProvider _serviceProvider;
        private IServiceScope _scope;

        public RabbitMQHostedServiceRegistrator(ILogger<RabbitMQHostedServiceRegistrator<TIntegrationEventHandler, TIntegrationEvent>> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateScope();

            _consumerHandler = 
                _scope.ServiceProvider.GetRequiredService<IHandlerRegistrator<TIntegrationEventHandler, TIntegrationEvent>>();

            _consumerHandler.Register();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stop {nameof(RabbitMQHostedServiceRegistrator<TIntegrationEventHandler, TIntegrationEvent>)}: Canceling {typeof(TIntegrationEventHandler).Name} as Consumer for Queue {typeof(TIntegrationEvent).Name}");

            _consumerHandler.UnRegister();

            _scope.Dispose();

            return Task.CompletedTask;
        }
    }
}
