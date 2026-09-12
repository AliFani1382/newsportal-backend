using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class NewsletterSubscriberRepository
    : Repository<NewsletterSubscriber>, INewsletterSubscriberRepository
{
    public NewsletterSubscriberRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<NewsletterSubscriber?> GetByEmailAsync(
        string email)
    {
        return await dbSet
            .FirstOrDefaultAsync(n => n.Email == email);
    }
}