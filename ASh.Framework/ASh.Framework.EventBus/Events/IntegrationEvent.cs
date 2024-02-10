using System.Text.Json.Serialization;

namespace ASh.Framework.EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
        }

        [JsonInclude]
        public Guid Id { get; set; }

        [JsonInclude]
        public DateTime CreatedOn { get; set; }
    }
}
