namespace ASh.Framework.Domain
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {
    }
}
