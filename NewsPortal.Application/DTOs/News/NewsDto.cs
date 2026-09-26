using NewsPortal.Application.DTOs.Tags;

namespace NewsPortal.Application.DTOs.News;

public record NewsDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public string? ImagePath { get; init; }
    public DateTime PublicationDate { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string Status { get; init; } = string.Empty;
    public int? CityId { get; init; }
    public string? CityName { get; init; }
    public int? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public int WriterId { get; init; }
    public string? WriterName { get; init; }
    public List<TagDto> Tags { get; init; } = [];
    public List<NewsImageDto> Images { get; init; } = [];
}
