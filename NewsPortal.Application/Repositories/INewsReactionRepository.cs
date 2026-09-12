using NewsPortal.Domain.Entities;
using NewsPortal.Domain.Enums;

namespace NewsPortal.Application.Repositories;

public interface INewsReactionRepository
    : IRepository<NewsReaction>
{
    Task<NewsReaction?> GetByUserAndNewsAsync(
        int userId,
        int newsId);

    Task<int> CountByNewsAndTypeAsync(
        int newsId,
        ReactionType type);
}