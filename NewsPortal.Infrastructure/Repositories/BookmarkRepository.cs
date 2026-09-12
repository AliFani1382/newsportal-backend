using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class BookmarkRepository
    : Repository<Bookmark>, IBookmarkRepository
{
    public BookmarkRepository(
        ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<Bookmark?> GetByUserAndNewsAsync(
        int userId,
        int newsId)
    {
        return await dbSet
            .FirstOrDefaultAsync(b =>
                b.UserId == userId &&
                b.NewsId == newsId);
    }

    public async Task<IReadOnlyList<Bookmark>> GetByUserIdAsync(
        int userId)
    {
        return await dbSet
            .AsNoTracking()
            .Include(b => b.News)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedDate)
            .ThenByDescending(b => b.Id)
            .ToListAsync();
    }
}