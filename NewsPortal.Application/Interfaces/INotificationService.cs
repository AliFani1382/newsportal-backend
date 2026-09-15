using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Notifications;

namespace NewsPortal.Application.Interfaces;

public interface INotificationService
{
    Task<ApiResponse<IReadOnlyList<NotificationDto>>> GetNotificationsAsync(
        int userId);

    Task<ApiResponse<UnreadCountDto>> GetUnreadCountAsync(
        int userId);

    Task<ApiResponse<bool>> MarkAsReadAsync(
        int notificationId,
        int userId);

    Task<ApiResponse<bool>> MarkAllAsReadAsync(
        int userId);

    Task CreateAsync(
        int userId,
        string title,
        string message,
        string? linkUrl = null);

    Task CreateManyAsync(
        IEnumerable<int> userIds,
        string title,
        string message,
        string? linkUrl = null);
}