using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Reactions;

namespace NewsPortal.Application.Interfaces;

public interface IReactionService
{
    Task<ApiResponse<ReactionDto>> GetReactionAsync(
        int newsId,
        int userId);

    Task<ApiResponse<ReactionDto>> SetReactionAsync(
        int newsId,
        int userId,
        string type);
}