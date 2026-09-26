namespace NewsPortal.Application.DTOs.Comments;

public record CommentDto
{
    public int Id { get; init; }

    public int NewsId { get; init; }

    public int UserId { get; init; }

    public required string UserName { get; init; }

    public required string Content { get; init; }

    public required string Status { get; init; }

    public DateTime CreatedAt { get; init; }
}