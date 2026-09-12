using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class NewsImageRepository
    : Repository<NewsImage>, INewsImageRepository
{
    public NewsImageRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<NewsImage>> GetByNewsIdAsync(
        int newsId)
    {
        return await dbSet
            .AsNoTracking()
            .Where(x => x.NewsId == newsId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .ToListAsync();
    }
}