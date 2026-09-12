using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface INewsletterSubscriberRepository
    : IRepository<NewsletterSubscriber>
{
    Task<NewsletterSubscriber?> GetByEmailAsync(string email);
}