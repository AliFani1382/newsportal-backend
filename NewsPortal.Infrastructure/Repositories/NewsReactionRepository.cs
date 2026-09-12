using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Domain.Enums;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class NewsReactionRepository
    : Repository<NewsReaction>, INewsReactionRepository
{
    public NewsReactionRepository(
        ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<NewsReaction?> GetByUserAndNewsAsync(
        int userId,
        int newsId)
    {
        return await dbSet
            .FirstOrDefaultAsync(r =>
                r.UserId == userId &&
                r.NewsId == newsId);
    }

    public async Task<int> CountByNewsAndTypeAsync(
        int newsId,
        ReactionType type)
    {
        return await dbSet
            .CountAsync(r =>
                r.NewsId == newsId &&
                r.Type == type);
    }
}