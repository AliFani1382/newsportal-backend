namespace NewsPortal.Application.DTOs.EmailVerification;

public record VerifyEmailResponseDto
{
    public bool IsVerified { get; init; }
}