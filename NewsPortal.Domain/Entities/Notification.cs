using NewsPortal.Domain.Common;

namespace NewsPortal.Domain.Entities;

public class Notification : BaseEntity
{
    public int UserId { get; private set; }

    public string Title { get; private set; } = null!;

    public string Message { get; private set; } = null!;

    public bool IsRead { get; private set; }

    public string? LinkUrl { get; private set; }

    public User User { get; private set; } = null!;

    private Notification()
    {
    }

    public Notification(
        int userId,
        string title,
        string message,
        string? linkUrl = null)
    {
        UserId = userId;
        Title = title;
        Message = message;
        LinkUrl = linkUrl;
        IsRead = false;
        CreatedDate = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        UpdatedAt = DateTime.UtcNow;
    }
}