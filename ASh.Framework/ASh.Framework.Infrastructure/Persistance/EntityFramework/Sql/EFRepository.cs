using ASh.Framework.Domain;
using ASh.Framework.Domain.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace ASh.Framework.Infrastructure.Persistance.EntityFramework.Sql
{
    public class EFRepository<T, TKey> : IRepository<T, TKey> where T : Entity<TKey>
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EFRepository(IDbContextAccessor contextAccessor)
        {
            _dbContext = contextAccessor.Context;
            _dbSet = _dbContext.Set<T>();
        }

     
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<Tuple<List<T>, int>> GetAsync(Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object?>>[]? includes = null,
            Expression<Func<T, object>>? order = null,
            int? skip = null, int? take = null, bool asc = true)
        {
            var query = _dbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            var count = await query.CountAsync();

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }


            if (order is not null)
            {
                query = asc ? query.OrderBy(order) : query.OrderByDescending(order);
            }
            else
            {
                // default order
                query = query.OrderBy(t => t.Id);
            }

            if (skip.HasValue)
            {
                query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query.Take(take.Value);
            }
            var res = await query.ToListAsync();
            return new Tuple<List<T>, int>(res, count);

        }


        public async Task<T?> GetByIdAsync(TKey id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
