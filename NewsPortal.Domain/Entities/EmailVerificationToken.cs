using NewsPortal.Domain.Common;

namespace NewsPortal.Domain.Entities;

public class EmailVerificationToken : BaseEntity
{
    public int UserId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }

    public User User { get; private set; } = null!;

    private EmailVerificationToken()
    {
    }

    public EmailVerificationToken(
        int userId,
        string token,
        DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        IsUsed = false;
        CreatedDate = DateTime.UtcNow;
    }

    public bool IsValid()
    {
        return !IsUsed && DateTime.UtcNow < ExpiresAt;
    }

    public void MarkAsUsed()
    {
        IsUsed = true;
        UpdatedAt = DateTime.UtcNow;
    }
}