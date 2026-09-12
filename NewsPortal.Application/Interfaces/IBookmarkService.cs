using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Bookmarks;
using NewsPortal.Application.DTOs.News;

namespace NewsPortal.Application.Interfaces;

public interface IBookmarkService
{
    Task<ApiResponse<IReadOnlyList<NewsDto>>> GetMyBookmarksAsync(
        int userId);

    Task<ApiResponse<bool>> AddAsync(
        int newsId,
        int userId);

    Task<ApiResponse<bool>> RemoveAsync(
        int newsId,
        int userId);

    Task<ApiResponse<BookmarkStatusDto>> GetStatusAsync(
        int newsId,
        int userId);
}