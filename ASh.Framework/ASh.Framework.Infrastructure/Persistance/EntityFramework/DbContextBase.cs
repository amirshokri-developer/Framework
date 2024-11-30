using Microsoft.EntityFrameworkCore;

namespace ASh.Framework.Infrastructure.Persistance.EntityFramework
{
    public abstract class DbContextBase : DbContext
    {
        protected DbContextBase(DbContextOptions options) : base(options)
        {

        }
    }
}
