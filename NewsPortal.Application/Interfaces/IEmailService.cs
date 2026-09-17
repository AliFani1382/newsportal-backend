namespace NewsPortal.Application.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(
        string email,
        string token);

    Task SendEmailVerificationEmailAsync(
        string email,
        string token);
}