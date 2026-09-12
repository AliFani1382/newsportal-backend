using NewsPortal.Domain.Common;
using NewsPortal.Domain.Enums;

namespace NewsPortal.Domain.Entities;

public class NewsReaction : BaseEntity
{
    public int NewsId { get; private set; }

    public int UserId { get; private set; }

    public ReactionType Type { get; private set; }

    public News News { get; private set; } = null!;

    public User User { get; private set; } = null!;

    private NewsReaction()
    {
    }

    public NewsReaction(
        int newsId,
        int userId,
        ReactionType type)
    {
        NewsId = newsId;
        UserId = userId;
        Type = type;
        CreatedDate = DateTime.UtcNow;
    }

    public void ChangeType(
        ReactionType type)
    {
        Type = type;
        UpdatedAt = DateTime.UtcNow;
    }
}