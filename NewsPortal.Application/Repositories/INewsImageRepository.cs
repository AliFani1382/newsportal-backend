using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface INewsImageRepository : IRepository<NewsImage>
{
    Task<IReadOnlyList<NewsImage>> GetByNewsIdAsync(int newsId);
}