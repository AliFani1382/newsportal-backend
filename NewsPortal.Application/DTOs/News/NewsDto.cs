using NewsPortal.Application.DTOs.Tags;

namespace NewsPortal.Application.DTOs.News;

public class NewsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public DateTime PublicationDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public string? CityName { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int WriterId { get; set; }
    public string? WriterName { get; set; }
    public List<TagDto> Tags { get; set; } = [];
    public List<NewsImageDto> Images { get; set; } = [];
}
