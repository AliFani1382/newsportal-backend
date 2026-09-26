namespace NewsPortal.Application.DTOs.Tags;

public record TagDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Slug { get; init; } = null!;
}