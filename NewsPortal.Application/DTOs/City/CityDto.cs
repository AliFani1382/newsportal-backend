namespace NewsPortal.Application.DTOs.City;

public record CityDto
{
    public int Id { get; init; }
    public string? Slug { get; init; }
    public required  string Name { get; init; } 
  
}
