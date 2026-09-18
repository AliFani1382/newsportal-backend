using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class RefreshTokenRepository
    : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<RefreshToken?> GetActiveTokenAsync(string token)
    {
        return await dbSet.FirstOrDefaultAsync(t =>
            t.Token == token &&
            !t.IsRevoked &&
            t.ExpiresAt > DateTime.UtcNow);
    }

    public async Task RevokeAllActiveForUserAsync(int userId)
    {
        var tokens = await dbSet
            .Where(t =>
                t.UserId == userId &&
                !t.IsRevoked &&
                t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.Revoke();
        }
    }
}