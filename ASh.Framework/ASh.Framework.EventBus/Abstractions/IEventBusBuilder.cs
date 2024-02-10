using Microsoft.Extensions.DependencyInjection;

namespace ASh.Framework.EventBus.Abstractions
{
    public interface IEventBusBuilder
    {
        public IServiceCollection Services { get; }
    }
}
