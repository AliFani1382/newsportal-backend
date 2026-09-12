namespace NewsPortal.Application.DTOs.Profile;

public class UserProfileDto
{
    public int Id { get; set; }
    public required string UserName { get; set; }

    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string Role { get; set; }
    public bool IsActive { get; set; }
}
