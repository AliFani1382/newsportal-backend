using NewsPortal.Domain.Entities;
using NewsPortal.Domain.Enums;

namespace NewsPortal.Application.Repositories;

public interface INewsRepository : IRepository<News>
{
    Task<News?> GetBySlugAsync(
        string slug);

    Task<bool> ExistsBySlugAsync(
        string slug,
        int? excludeId = null);

    Task<IReadOnlyList<News>> GetLatestAsync(
        int count);

    Task<IReadOnlyList<News>> SearchAsync(
        string query);

    Task<IReadOnlyList<News>> GetByCityIdAsync(
        int cityId);

    Task<IReadOnlyList<News>> GetByCategoryIdAsync(
        int categoryId);

    Task<IReadOnlyList<News>> GetRelatedAsync(
    int newsId,
    int categoryId,
    int count);

    Task IncrementViewCountAsync(
    int newsId);

    Task<IReadOnlyList<News>> GetPopularAsync(
    int count);

    Task<IReadOnlyList<News>> GetFeaturedAsync(
    int count);

    Task<(IReadOnlyList<News> Items, int TotalCount)> GetPagedNewsAsync(
        int pageNumber,
        int pageSize,
        int? categoryId,
        int? cityId,
        string? search,
        NewsStatus? statusFilter = null);
}