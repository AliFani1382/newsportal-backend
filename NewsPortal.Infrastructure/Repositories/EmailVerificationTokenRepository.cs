using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class EmailVerificationTokenRepository
    : Repository<EmailVerificationToken>, IEmailVerificationTokenRepository
{
    public EmailVerificationTokenRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<EmailVerificationToken?> GetValidTokenAsync(
        string token)
    {
        return await dbSet
            .FirstOrDefaultAsync(t =>
                t.Token == token &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow);
    }
    public async Task<IReadOnlyList<EmailVerificationToken>> GetValidTokensByUserIdAsync(
    int userId)
    {
        return await dbSet
            .Where(t =>
                t.UserId == userId &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }
}