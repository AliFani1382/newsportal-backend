using System.Linq.Expressions;

namespace NewsPortal.Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> FirstOrDefaultAsync(
      Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

    }
}

