using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class TagRepository
    : Repository<Tag>, ITagRepository
{
    public TagRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await dbSet
            .FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<Tag?> GetBySlugAsync(string slug)
    {
        return await dbSet
            .FirstOrDefaultAsync(t => t.Slug == slug);
    }
    public async Task<IReadOnlyList<Tag>> GetByIdsAsync(
    IReadOnlyList<int> ids)
    {
        if (ids.Count == 0)
            return [];

        return await dbSet
            .Where(t => ids.Contains(t.Id))
            .ToListAsync();
    }
}