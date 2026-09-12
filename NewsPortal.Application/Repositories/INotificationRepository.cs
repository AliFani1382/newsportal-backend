using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IReadOnlyList<Notification>> GetByUserIdAsync(int userId);

    Task<int> GetUnreadCountAsync(int userId);

    Task<Notification?> GetByIdAndUserIdAsync(
        int notificationId,
        int userId);
}