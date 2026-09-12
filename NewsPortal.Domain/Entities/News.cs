using NewsPortal.Domain.Common;
using NewsPortal.Domain.Enums;

namespace NewsPortal.Domain.Entities;

public class News : BaseEntity
{
    public required string Title { get; set; }

    public required string Slug { get; set; }

    public required string Content { get; set; }

    public DateTime PublicationDate { get; set; }

    public string? ImagePath { get; set; }

    public NewsStatus Status { get; set; } = NewsStatus.Draft;
    public int ViewCount { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;

    public int? CityId { get; set; }
    public City? City { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    public int WriterId { get; set; }
    public User Writer { get; set; } = null!;
    public ICollection<NewsTag> NewsTags { get; set; } = [];
    public ICollection<NewsImage> NewsImages { get; set; } = [];

    public void Update(
        string title,
        string content,
        int categoryId,
        int? cityId)
    {
        Title = title;
        Content = content;
        CategoryId = categoryId;
        CityId = cityId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeImage(string? imagePath)
    {
        ImagePath = imagePath;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(NewsStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}