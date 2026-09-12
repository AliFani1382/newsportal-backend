using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IReadOnlyList<Comment>> GetByNewsIdAsync(
        int newsId,
        bool includeAllStatuses = false);

    Task<IReadOnlyList<Comment>> GetPendingAsync();

    Task<Comment?> GetByIdWithUserAsync(
        int id);
}