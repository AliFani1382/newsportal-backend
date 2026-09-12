using NewsPortal.Domain.Common;

namespace NewsPortal.Domain.Entities;

public class Bookmark : BaseEntity
{
    public int NewsId { get; private set; }

    public int UserId { get; private set; }

    public News News { get; private set; } = null!;

    public User User { get; private set; } = null!;

    private Bookmark()
    {
    }

    public Bookmark(
        int newsId,
        int userId)
    {
        NewsId = newsId;
        UserId = userId;
        CreatedDate = DateTime.UtcNow;
    }
}