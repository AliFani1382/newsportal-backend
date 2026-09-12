using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public override Task<User?> GetByIdAsync(int id)
    {
        return dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public override async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await dbSet
            .Include(u => u.Role)
            .ToListAsync();
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return dbSet
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<User?> GetByUsernameOrEmailAsync(
        string usernameOrEmail)
    {
        return dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u =>
                u.Username == usernameOrEmail ||
                u.Email == usernameOrEmail);
    }

    public Task<bool> ExistsByUsernameAsync(
        string username)
    {
        return dbSet
            .AnyAsync(u => u.Username == username);
    }

    public Task<bool> ExistsByEmailAsync(
        string email)
    {
        return dbSet
            .AnyAsync(u => u.Email == email);
    }


    public Task<bool> ExistsAsync(
        string username,
        string email)
    {
        return dbSet
            .AnyAsync(u =>
                u.Username == username ||
                u.Email == email);
    }

    public Task<bool> ExistsByUsernameAsync(
        string username,
        int excludeUserId)
    {
        return dbSet.AnyAsync(u =>
            u.Username == username &&
            u.Id != excludeUserId);
    }

    public Task<bool> ExistsByEmailAsync(
        string email,
        int excludeUserId)
    {
        return dbSet.AnyAsync(u =>
            u.Email == email &&
            u.Id != excludeUserId);
    }

   
}