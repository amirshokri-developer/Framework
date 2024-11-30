namespace ASh.Framework.Infrastructure.Persistance.EntityFramework
{
    public class DbContextAccessor : IDbContextAccessor
    {
        public DbContextAccessor(DbContextBase context)
        {
            Context = context;
        }

        public DbContextBase Context { get; }
    }
}
