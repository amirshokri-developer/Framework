using System.Text.Json.Serialization.Metadata;
using System.Text.Json;

namespace ASh.Framework.EventBus.Abstractions
{
    public class EventBusSubscriptionInfo
    {
        public Dictionary<string, Type> EventTypes { get; } = [];

        public JsonSerializerOptions JsonSerializerOptions { get; } = new(DefaultSerializerOptions);

        internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
        {
            TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault
                                ? new DefaultJsonTypeInfoResolver() 
                                : JsonTypeInfoResolver.Combine()
        };
    }
}
