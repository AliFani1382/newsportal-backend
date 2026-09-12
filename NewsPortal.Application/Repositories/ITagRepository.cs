using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetByNameAsync(string name);

    Task<Tag?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<Tag>> GetByIdsAsync(
    IReadOnlyList<int> ids);
}