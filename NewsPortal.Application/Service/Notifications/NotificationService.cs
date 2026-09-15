using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Notifications;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Service.Notifications;

public sealed class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IReadOnlyList<NotificationDto>>> GetNotificationsAsync(
        int userId)
    {
        var notifications =
            await _notificationRepository.GetByUserIdAsync(userId);

        var result = notifications
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedDate,
                LinkUrl = n.LinkUrl
            })
            .ToList();

        return ApiResponse<IReadOnlyList<NotificationDto>>.Success(
            result);
    }

    public async Task<ApiResponse<UnreadCountDto>> GetUnreadCountAsync(
        int userId)
    {
        var count =
            await _notificationRepository.GetUnreadCountAsync(userId);

        return ApiResponse<UnreadCountDto>.Success(
            new UnreadCountDto
            {
                Count = count
            });
    }

    public async Task<ApiResponse<bool>> MarkAsReadAsync(
        int notificationId,
        int userId)
    {
        var notification =
            await _notificationRepository.GetByIdAndUserIdAsync(
                notificationId,
                userId);

        if (notification is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "اعلان مورد نظر پیدا نشد.");
        }

        notification.MarkAsRead();

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "اعلان به عنوان خوانده شده علامت‌گذاری شد.");
    }

    public async Task<ApiResponse<bool>> MarkAllAsReadAsync(
        int userId)
    {
        var notifications =
            await _notificationRepository.GetByUserIdAsync(userId);

        foreach (var notification in notifications)
        {
            if (!notification.IsRead)
            {
                notification.MarkAsRead();
            }
        }

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "همه اعلان‌ها به عنوان خوانده شده علامت‌گذاری شدند.");
    }

    public async Task CreateAsync(
        int userId,
        string title,
        string message,
        string? linkUrl = null)
    {
        var notification =
            new Notification(
                userId,
                title,
                message,
                linkUrl);

        await _notificationRepository.AddAsync(
            notification);

        await _unitOfWork.CommitAsync();
    }

    public async Task CreateManyAsync(
        IEnumerable<int> userIds,
        string title,
        string message,
        string? linkUrl = null)
    {
        var notifications =
            userIds
                .Distinct()
                .Select(userId =>
                    new Notification(
                        userId,
                        title,
                        message,
                        linkUrl))
                .ToList();

        if (notifications.Count == 0)
        {
            return;
        }

        foreach (var notification in notifications)
        {
            await _notificationRepository.AddAsync(
                notification);
        }

        await _unitOfWork.CommitAsync();
    }
}