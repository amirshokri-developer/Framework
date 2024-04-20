using ASh.Framework.EventBus.Abstractions;
using ASh.Framework.EventBus.Events;
using ASh.Framework.EventBus.RabbitMQ.Abstractions;
using ASh.Framework.EventBus.RabbitMQ.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace ASh.Framework.EventBus.RabbitMQ.Extensions
{
    public static class RabbitMQRegistration
    {

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {

            var section = configuration.GetSection("RabbitMQ");

            if (section is null)
                throw new ArgumentNullException(nameof(section));

            var validOption = section.Get<RabbitMQOption>();
            ArgumentNullException.ThrowIfNull(validOption);

            var settings = validOption;


            services.AddSingleton(provider =>
                {
                    var factory = new ConnectionFactory
                    {
                        UserName = settings.UserName,
                        Password = settings.Password,
                        HostName = settings.Host,
                        Port = settings.Port,

                        DispatchConsumersAsync = true,
                        AutomaticRecoveryEnabled = true,
                    };

                    return factory;
                });

            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddScoped<IChannelProvider, ChannelProvider>();
            services.AddScoped(typeof(IQueueChannelProvider<>), typeof(RabbitMQChannelProvider<>));
            services.AddScoped(typeof(IProducer<>), typeof(RabbitMQProducer<>));
            return services;
        }

        public static void AddConsumer<TIntegrationEventHandler, TEventIntegration>(this IServiceCollection services) 
            where TIntegrationEventHandler : IIntegrationEventHandler<TEventIntegration> where TEventIntegration : IntegrationEvent
        {
            services.AddScoped(typeof(TIntegrationEventHandler));
            services.AddScoped<IHandlerRegistrator<TIntegrationEventHandler, TEventIntegration>, HandlerRegisterator<TIntegrationEventHandler, TEventIntegration>>();
            services.AddHostedService<RabbitMQHostedServiceRegistrator<TIntegrationEventHandler, TEventIntegration>>();
        }

   
    }

}
