using NewsPortal.Domain.Common;

namespace NewsPortal.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public int RoleId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsEmailVerified { get; private set; }

    public Role Role { get; private set; } = null!;

    public ICollection<News> News { get; private set; } = [];

    public ICollection<Comment> Comments { get; private set; } = [];

    private User()
    {
    }

    public User(
        string username,
        string email,
        string passwordHash,
        string fullName,
        int roleId)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        RoleId = roleId;
        IsActive = true;
        IsEmailVerified = false;
    }

    public void UpdateProfile(
        string fullName,
        string email)
    {
        FullName = fullName;
        Email = email;
    }
    public void VerifyEmail()
    {
        IsEmailVerified = true;
    }

    public void ChangePassword(
        string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void ChangeRole(
        int roleId)
    {
        RoleId = roleId;
    }
}