using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.News;

namespace NewsPortal.Application.Service.News;

public interface INewsService
{
    Task<ApiResponse<PagedResult<NewsDto>>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        int? categoryId,
        int? cityId,
        string? search,
        bool isAdmin);

    Task<ApiResponse<NewsDto>> GetByIdAsync(
        int id,
        int? requestingUserId,
        bool isAdmin);

    Task<ApiResponse<NewsDto>> GetBySlugAsync(
        string slug,
        int? requestingUserId,
        bool isAdmin);

    Task<ApiResponse<bool>> IncrementViewCountAsync(
    int newsId);

    Task<ApiResponse<List<NewsDto>>> GetRelatedAsync(
    int newsId,
    int count);

    Task<ApiResponse<List<NewsDto>>> GetPopularAsync(
    int count);
    Task<ApiResponse<List<NewsDto>>> GetFeaturedAsync(
    int count);
    Task<ApiResponse<bool>> SetFeaturedAsync(
    int newsId,
    bool isFeatured);

    Task<ApiResponse<NewsDto>> CreateAsync(
        CreateNewsDto dto,
        int userId,
        bool isAdmin);

    Task<ApiResponse<NewsDto>> UpdateAsync(
        int id,
        UpdateNewsDto dto,
        int userId,
        bool isAdmin);

    Task<ApiResponse<bool>> DeleteAsync(
        int id,
        int userId,
        bool isAdmin);

    Task<ApiResponse<NewsDto>> ChangeStatusAsync(
        int id,
        ChangeNewsStatusDto dto);
}