namespace NewsPortal.Application.DTOs.Reactions;

public class ReactionDto
{
    public int LikeCount { get; set; }

    public int DislikeCount { get; set; }

    public string? MyReaction { get; set; }
}