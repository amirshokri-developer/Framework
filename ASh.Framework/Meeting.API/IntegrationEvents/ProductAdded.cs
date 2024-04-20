using ASh.Framework.EventBus.Events;

namespace Meeting.API.IntegrationEvents
{
    public class ProductAdded : IntegrationEvent
    {
        public ProductAdded(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}
