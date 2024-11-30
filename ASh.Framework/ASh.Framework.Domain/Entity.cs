namespace ASh.Framework.Domain
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; protected set; }       

    }

   
}
