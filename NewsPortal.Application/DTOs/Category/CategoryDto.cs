namespace NewsPortal.Application.DTOs.Category;

public record CategoryDto
{
    public int Id { get; init; }
    public string Slug { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
   
}
