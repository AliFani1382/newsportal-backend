using NewsPortal.Domain.Common;
using NewsPortal.Domain.Enums;

namespace NewsPortal.Domain.Entities;

public class Comment : BaseEntity
{
    public int NewsId { get; private set; }

    public int UserId { get; private set; }

    public string Content { get; private set; } = null!;

    public CommentStatus Status { get; private set; }

    public News News { get; private set; } = null!;

    public User User { get; private set; } = null!;

    private Comment() 
    {
    }

    public Comment(
        int newsId,
        int userId,
        string content)
    {
        NewsId = newsId;
        UserId = userId;
        Content = content;
        Status = CommentStatus.Pending;
        CreatedDate = DateTime.UtcNow;
    }

    public void Approve()
    {
        Status = CommentStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = CommentStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }
}