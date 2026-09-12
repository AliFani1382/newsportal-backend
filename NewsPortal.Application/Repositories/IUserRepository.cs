using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);

    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);

    Task<bool> ExistsByUsernameAsync(string username);

    Task<bool> ExistsByEmailAsync(string email);

    Task<bool> ExistsAsync(string username, string email);

    Task<bool> ExistsByUsernameAsync(
        string username,
        int excludeUserId);

    Task<bool> ExistsByEmailAsync(
        string email,
        int excludeUserId);
}