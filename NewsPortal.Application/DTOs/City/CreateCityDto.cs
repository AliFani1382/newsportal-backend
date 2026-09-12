namespace NewsPortal.Application.DTOs.City;

public class CreateCityDto
{
    public required string Name { get; set; } 
    public string? Slug { get; set; }

}
