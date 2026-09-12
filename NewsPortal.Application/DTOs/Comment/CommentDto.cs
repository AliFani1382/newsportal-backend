namespace NewsPortal.Application.DTOs.Comments;

public class CommentDto
{
    public int Id { get; set; }

    public int NewsId { get; set; }

    public int UserId { get; set; }

    public required string UserName { get; set; }

    public required string Content { get; set; }

    public required string Status { get; set; }

    public DateTime CreatedAt { get; set; }
}