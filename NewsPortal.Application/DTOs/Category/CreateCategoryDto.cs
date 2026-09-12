namespace NewsPortal.Application.DTOs.Category;

public class CreateCategoryDto
{
    public string? Slug { get; set; }

    public required string Name { get; set; }

}
