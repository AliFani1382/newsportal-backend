using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Comments;

namespace NewsPortal.Application.Interfaces;

public interface ICommentService
{
    Task<ApiResponse<IReadOnlyList<CommentDto>>> GetByNewsIdAsync(
        int newsId,
        bool isAdmin);

    Task<ApiResponse<CommentDto>> CreateAsync(
        int newsId,
        CreateCommentDto dto,
        int userId);

    Task<ApiResponse<IReadOnlyList<CommentDto>>> GetPendingAsync();

    Task<ApiResponse<bool>> ApproveAsync(
        int id);

    Task<ApiResponse<bool>> RejectAsync(
        int id);

    Task<ApiResponse<bool>> DeleteAsync(
        int id,
        int userId,
        bool isAdmin);
}