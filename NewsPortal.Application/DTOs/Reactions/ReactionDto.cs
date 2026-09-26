namespace NewsPortal.Application.DTOs.Reactions;

public record  ReactionDto
{
    public int LikeCount { get; init; }

    public int DislikeCount { get; init; }

    public string? MyReaction { get; init; }
}