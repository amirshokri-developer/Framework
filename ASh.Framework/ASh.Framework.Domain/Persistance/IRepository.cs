using System.Linq.Expressions;

namespace ASh.Framework.Domain.Persistance
{
    public interface IRepository<T, TKey> where T : IEntity<TKey>
    {
        Task<Tuple<List<T>, int>> GetAsync(Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object?>>[]? includes = null,
            Expression<Func<T, object>>? order = null,
            int? skip = null, int? take = null, bool asc = true);
        Task<T?> GetByIdAsync(TKey id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
