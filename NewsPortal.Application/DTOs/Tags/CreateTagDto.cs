namespace NewsPortal.Application.DTOs.Tags;

public class CreateTagDto
{
    public required string Name { get; set; }

    public required string Slug { get; set; }
}