using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface IBookmarkRepository
    : IRepository<Bookmark>
{
    Task<Bookmark?> GetByUserAndNewsAsync(
        int userId,
        int newsId);

    Task<IReadOnlyList<Bookmark>> GetByUserIdAsync(
        int userId);
}