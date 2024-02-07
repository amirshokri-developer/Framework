namespace ASh.Framework.Domain
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
