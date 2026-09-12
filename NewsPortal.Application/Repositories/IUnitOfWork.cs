using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace NewsPortal.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        INewsRepository News { get; }
        ICityRepository Cities { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
