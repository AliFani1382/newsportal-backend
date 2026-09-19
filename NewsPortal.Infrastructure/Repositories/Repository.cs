using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace NewsPortal.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDBContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDBContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync(
            Expression<Func<T, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public virtual void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public virtual async Task<IEnumerable<T>> GetFilteredAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            foreach (var includeProperty in includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public virtual void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
