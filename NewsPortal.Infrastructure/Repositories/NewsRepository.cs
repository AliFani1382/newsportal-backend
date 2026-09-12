using Microsoft.EntityFrameworkCore;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Domain.Enums;
using NewsPortal.Infrastructure.Persistence;

namespace NewsPortal.Infrastructure.Repositories;

public sealed class NewsRepository : Repository<News>, INewsRepository
{
    public NewsRepository(ApplicationDBContext context)
        : base(context)
    {
    }

    public override async Task<News?> GetByIdAsync(int id)
    {
        return await dbSet
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<News?> GetBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return null;

        slug = slug.Trim();

        return await dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .FirstOrDefaultAsync(n => n.Slug == slug);
    }

    public async Task<bool> ExistsBySlugAsync(
        string slug,
        int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return false;

        slug = slug.Trim();

        return await dbSet.AnyAsync(n =>
            n.Slug == slug &&
            (!excludeId.HasValue || n.Id != excludeId.Value));
    }

    public async Task<IReadOnlyList<News>> GetLatestAsync(
        int count)
    {
        if (count <= 0)
            return [];

        return await dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .Where(n => n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublicationDate)
            .ThenByDescending(n => n.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<News>> SearchAsync(
        string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        query = query.Trim();

        return await dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .Where(n =>
                n.Status == NewsStatus.Published &&
                (n.Title.Contains(query) ||
                 n.Content.Contains(query)))
            .OrderByDescending(n => n.PublicationDate)
            .ThenByDescending(n => n.Id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<News>> GetByCityIdAsync(
        int cityId)
    {
        return await dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .Where(n =>
                n.CityId == cityId &&
                n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublicationDate)
            .ThenByDescending(n => n.Id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<News>> GetByCategoryIdAsync(
        int categoryId)
    {
        return await dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .Where(n =>
                n.CategoryId == categoryId &&
                n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublicationDate)
            .ThenByDescending(n => n.Id)
            .ToListAsync();
    }
    public async Task<IReadOnlyList<News>> GetRelatedAsync(
    int newsId,
    int categoryId,
    int count)
    {
        if (count <= 0)
            return [];

        return await dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .Where(n =>
                n.Id != newsId &&
                n.CategoryId == categoryId &&
                n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublicationDate)
            .ThenByDescending(n => n.Id)
            .Take(count)
            .ToListAsync();
    }
    public async Task IncrementViewCountAsync(
    int newsId)
    {
        await dbSet
            .Where(n => n.Id == newsId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(
                    n => n.ViewCount,
                    n => n.ViewCount + 1));
    }

    public async Task<IReadOnlyList<News>> GetPopularAsync(
    int count)
    {
        if (count <= 0)
            return [];

        return await dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .Where(n =>
                n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.ViewCount)
            .ThenByDescending(n => n.PublicationDate)
            .ThenByDescending(n => n.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<News>> GetFeaturedAsync(
    int count)
    {
        if (count <= 0)
            return [];

        return await dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages)
            .Where(n =>
                n.IsFeatured &&
                n.Status == NewsStatus.Published)
            .OrderByDescending(n => n.PublicationDate)
            .ThenByDescending(n => n.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<(IReadOnlyList<News> Items, int TotalCount)>
        GetPagedNewsAsync(
            int pageNumber,
            int pageSize,
            int? categoryId,
            int? cityId,
            string? search,
            NewsStatus? statusFilter = null)
    {
        pageNumber = pageNumber < 1
            ? 1
            : pageNumber;

        pageSize = pageSize < 1
            ? 10
            : Math.Min(pageSize, 100);

        search = string.IsNullOrWhiteSpace(search)
            ? null
            : search.Trim();

        IQueryable<News> query = dbSet
            .AsNoTracking()
            .Include(n => n.City)
            .Include(n => n.Category)
            .Include(n => n.Writer)
            .Include(n => n.NewsTags)
                .ThenInclude(nt => nt.Tag)
            .Include(n => n.NewsImages);

        if (statusFilter.HasValue)
        {
            query = query.Where(n =>
                n.Status == statusFilter.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(n =>
                n.CategoryId == categoryId.Value);
        }

        if (cityId.HasValue)
        {
            query = query.Where(n =>
                n.CityId == cityId.Value);
        }

        if (search is not null)
        {
            query = query.Where(n =>
                n.Title.Contains(search) ||
                n.Content.Contains(search));
        }

        var totalCount =
            await query.CountAsync();

        var items =
            await query
                .OrderByDescending(n => n.PublicationDate)
                .ThenByDescending(n => n.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        return (items, totalCount);
    }
}

