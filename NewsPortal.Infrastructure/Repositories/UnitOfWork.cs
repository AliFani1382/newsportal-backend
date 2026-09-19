using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;

        public INewsRepository News { get; }
        public ICityRepository Cities { get; }
        public ICategoryRepository Categories { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(
            ApplicationDBContext context,
            INewsRepository news,
            ICityRepository cities,
            ICategoryRepository categories,
            IUserRepository users)
        {
            _context = context;
            News = news;
            Cities = cities;
            Categories = categories;
            Users = users;
        }

        public Task<int> CommitAsync()
            => _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}

