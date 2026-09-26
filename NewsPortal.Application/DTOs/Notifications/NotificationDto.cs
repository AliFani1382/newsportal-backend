namespace NewsPortal.Application.DTOs.Notifications;

public record NotificationDto
{
    public int Id { get; init; }

    public string Title { get; init; } = null!;

    public string Message { get; init; } = null!;

    public bool IsRead { get; init; }

    public DateTime CreatedAt { get; init; }

    public string? LinkUrl { get; init; }
}