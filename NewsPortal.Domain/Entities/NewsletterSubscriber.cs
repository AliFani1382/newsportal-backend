using NewsPortal.Domain.Common;

namespace NewsPortal.Domain.Entities;

public class NewsletterSubscriber : BaseEntity
{
    public string Email { get; private set; } = null!;

    public bool IsActive { get; private set; }

    private NewsletterSubscriber()
    {
    }

    public NewsletterSubscriber(string email)
    {
        Email = email;
        IsActive = true;
        CreatedDate = DateTime.UtcNow;
    }

    public void Unsubscribe()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resubscribe()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}