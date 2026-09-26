namespace NewsPortal.Application.DTOs.Profile;

public record UserProfileDto
{
    public int Id { get; init; }
    public required string UserName { get; init; }

    public required string Email { get; init; }
    public required string FullName { get; init; }
    public required string Role { get; init; }
    public bool IsActive { get; init; }
}
