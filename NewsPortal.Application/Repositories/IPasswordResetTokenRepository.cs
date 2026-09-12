using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface IPasswordResetTokenRepository
    : IRepository<PasswordResetToken>
{
    Task<PasswordResetToken?> GetValidTokenAsync(
        string token);
}