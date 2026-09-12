namespace NewsPortal.Application.DTOs.PasswordReset;

public class ResetPasswordDto
{
    public required string Token { get; set; }

    public required string NewPassword { get; set; }
}