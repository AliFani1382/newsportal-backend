using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface IEmailVerificationTokenRepository
    : IRepository<EmailVerificationToken>
{
    Task<EmailVerificationToken?> GetValidTokenAsync(string token);

    Task<IReadOnlyList<EmailVerificationToken>> GetValidTokensByUserIdAsync(
        int userId);
}