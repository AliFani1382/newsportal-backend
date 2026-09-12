namespace NewsPortal.Application.DTOs.News;

public class NewsImageDto
{
    public int Id { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}