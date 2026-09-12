using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Domain.Enums;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class CommentRepository
    : Repository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<Comment>> GetByNewsIdAsync(
        int newsId,
        bool includeAllStatuses = false)
    {
        IQueryable<Comment> query = dbSet
            .AsNoTracking()
            .Include(c => c.User);

        query = query.Where(c =>
            c.NewsId == newsId);

        if (!includeAllStatuses)
        {
            query = query.Where(c =>
                c.Status == CommentStatus.Approved);
        }

        return await query
            .OrderByDescending(c => c.CreatedDate)
            .ThenByDescending(c => c.Id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Comment>> GetPendingAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Include(c => c.User)
            .Include(c => c.News)
            .Where(c =>
                c.Status == CommentStatus.Pending)
            .OrderBy(c => c.CreatedDate)
            .ThenBy(c => c.Id)
            .ToListAsync();
    }

    public async Task<Comment?> GetByIdWithUserAsync(
        int id)
    {
        return await dbSet
            .Include(c => c.User)
            .Include(c => c.News)
            .FirstOrDefaultAsync(c =>
                c.Id == id);
    }
}