using ASh.Framework.EventBus.Abstractions;

namespace Meeting.API.IntegrationEvents.Handlers
{
    public class ProductAddedEventHandler : IIntegrationEventHandler<ProductAdded>
    {
        public async Task HandleAsync(ProductAdded @event)
        {
            Console.WriteLine($"Product with name {@event.Name} handled successfully");
        }
    }
}
