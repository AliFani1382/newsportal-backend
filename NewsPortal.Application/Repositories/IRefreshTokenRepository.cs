using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetActiveTokenAsync(string token);
    Task RevokeAllActiveForUserAsync(int userId);
}