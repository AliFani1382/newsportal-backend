using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.Notifications;
using NewsPortal.Application.Interfaces;
using System.Security.Claims;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(
        INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = GetUserId();

        var response =
            await _notificationService.GetNotificationsAsync(userId);

        return response.ToHttpResult();
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = GetUserId();

        var response =
            await _notificationService.GetUnreadCountAsync(userId);

        return response.ToHttpResult();
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = GetUserId();

        var response =
            await _notificationService.MarkAsReadAsync(
                id,
                userId);

        return response.ToHttpResult();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = GetUserId();

        var response =
            await _notificationService.MarkAllAsReadAsync(userId);

        return response.ToHttpResult();
    }

    private int GetUserId()
    {
        return int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}