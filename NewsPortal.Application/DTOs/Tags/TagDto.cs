namespace NewsPortal.Application.DTOs.Tags;

public class TagDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;
}