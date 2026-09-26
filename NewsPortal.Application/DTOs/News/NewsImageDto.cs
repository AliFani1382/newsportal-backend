namespace NewsPortal.Application.DTOs.News;

public record NewsImageDto
{
    public int Id { get; init; }

    public string ImagePath { get; init; } = string.Empty;

    public int DisplayOrder { get; init; }
}