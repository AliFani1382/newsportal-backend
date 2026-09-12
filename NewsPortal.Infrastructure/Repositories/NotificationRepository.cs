using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class NotificationRepository
    : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<Notification>> GetByUserIdAsync(
        int userId)
    {
        return await dbSet
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedDate)
            .ThenByDescending(n => n.Id)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await dbSet.CountAsync(n =>
            n.UserId == userId &&
            !n.IsRead);
    }

    public async Task<Notification?> GetByIdAndUserIdAsync(
        int notificationId,
        int userId)
    {
        return await dbSet.FirstOrDefaultAsync(n =>
            n.Id == notificationId &&
            n.UserId == userId);
    }
}