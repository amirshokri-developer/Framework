namespace ASh.Framework.Domain
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; protected set; }

        public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

        public void QueueDomainEvent(IDomainEvent @event)
        {
            DomainEvents.Add(@event);
        }

    }

   
}
