namespace ASh.Framework.Infrastructure.Persistance.EntityFramework
{
    public interface IDbContextAccessor
    {
        DbContextBase Context { get; }
    }
}
