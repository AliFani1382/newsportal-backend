using Microsoft.AspNetCore.Http;

namespace NewsPortal.Application.DTOs.News;

public class UpdateNewsDto
{
    public required string Title { get; set; }

    public required string Content { get; set; }

    public int CategoryId { get; set; }

    public int? CityId { get; set; }

    public List<IFormFile> ImageFiles { get; set; } = [];

    public List<int> TagIds { get; set; } = [];
}

