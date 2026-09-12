using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.News;
using NewsPortal.Application.DTOs.Tags;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;
using NewsPortal.Domain.Enums;
using NewsEntity = NewsPortal.Domain.Entities.News;

namespace NewsPortal.Application.Service.News;

public sealed class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICityRepository _cityRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly IImageValidator _imageValidator;
    private readonly ISlugService _slugService;
    private readonly INewsImageRepository _newsImageRepository;

public NewsService(
    INewsRepository newsRepository,
    ICategoryRepository categoryRepository,
    ICityRepository cityRepository,
    ITagRepository tagRepository,
    IUnitOfWork unitOfWork,
    IFileService fileService,
    IImageValidator imageValidator,
    ISlugService slugService,
    INewsImageRepository newsImageRepository)
    {
        _newsRepository = newsRepository;
        _categoryRepository = categoryRepository;
        _cityRepository = cityRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _imageValidator = imageValidator;
        _slugService = slugService;
        _newsImageRepository = newsImageRepository;
    }

    public async Task<ApiResponse<NewsDto>> GetByIdAsync(
        int id,
        int? requestingUserId,
        bool isAdmin)
    {
        var news =
            await _newsRepository.GetByIdAsync(id);

        if (news is null)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        var canViewUnpublished =
            isAdmin ||
            (requestingUserId.HasValue &&
             news.WriterId == requestingUserId.Value);

        if (news.Status != NewsStatus.Published && !canViewUnpublished)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        return ApiResponse<NewsDto>.Success(
            MapToDto(news));
    }

    public async Task<ApiResponse<NewsDto>> GetBySlugAsync(
        string slug,
        int? requestingUserId,
        bool isAdmin)
    {
        var news =
            await _newsRepository.GetBySlugAsync(slug);

        if (news is null)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        var canViewUnpublished =
            isAdmin ||
            (requestingUserId.HasValue &&
             news.WriterId == requestingUserId.Value);

        if (news.Status != NewsStatus.Published && !canViewUnpublished)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        return ApiResponse<NewsDto>.Success(
            MapToDto(news));
    }

    public async Task<ApiResponse<bool>> IncrementViewCountAsync(
    int newsId)
    {
        var news =
            await _newsRepository.GetByIdAsync(newsId);

        if (news is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        await _newsRepository.IncrementViewCountAsync(
            newsId);

        return ApiResponse<bool>.Success(true);
    }

    public async Task<ApiResponse<List<NewsDto>>> GetRelatedAsync(
    int newsId,
    int count)
    {
        var news =
            await _newsRepository.GetByIdAsync(newsId);

        if (news is null)
        {
            return ApiResponse<List<NewsDto>>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        count = count < 1
            ? 5
            : Math.Min(count, 20);

        var relatedNews =
            await _newsRepository.GetRelatedAsync(
                newsId,
                news.CategoryId ?? 0,
                count);

        var result =
            relatedNews
                .Select(MapToDto)
                .ToList();

        return ApiResponse<List<NewsDto>>.Success(
            result);
    }

    public async Task<ApiResponse<List<NewsDto>>> GetPopularAsync(
    int count)
    {
        count = count < 1
            ? 5
            : Math.Min(count, 20);

        var popularNews =
            await _newsRepository.GetPopularAsync(
                count);

        var result =
            popularNews
                .Select(MapToDto)
                .ToList();

        return ApiResponse<List<NewsDto>>.Success(
            result);
    }

    public async Task<ApiResponse<List<NewsDto>>> GetFeaturedAsync(
    int count)
    {
        count = count < 1
            ? 5
            : Math.Min(count, 20);

        var featuredNews =
            await _newsRepository.GetFeaturedAsync(
                count);

        var result =
            featuredNews
                .Select(MapToDto)
                .ToList();

        return ApiResponse<List<NewsDto>>.Success(
            result);
    }


    public async Task<ApiResponse<bool>> SetFeaturedAsync(
     int newsId,
     bool isFeatured)
    {
        var news =
            await _newsRepository.GetByIdAsync(
                newsId);

        if (news is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "خبر مورد نظر پیدا نشد");
        }

        news.IsFeatured = isFeatured;
        news.UpdatedAt = DateTime.UtcNow;

        _newsRepository.Update(news);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(true);
    }
    

    public async Task<ApiResponse<PagedResult<NewsDto>>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        int? categoryId,
        int? cityId,
        string? search,
        bool isAdmin)
    {
        pageNumber = pageNumber < 1
            ? 1
            : pageNumber;

        pageSize = pageSize < 1
            ? 10
            : Math.Min(pageSize, 100);

        NewsStatus? statusFilter =
            isAdmin ? null : NewsStatus.Published;

        var result =
            await _newsRepository.GetPagedNewsAsync(
                pageNumber,
                pageSize,
                categoryId,
                cityId,
                search,
                statusFilter);

        var items = result.Items
            .Select(MapToDto)
            .ToList();

        var pagedResult =
            new PagedResult<NewsDto>(
                items,
                result.TotalCount,
                pageNumber,
                pageSize);

        return ApiResponse<PagedResult<NewsDto>>
            .Success(pagedResult);
    }

    public async Task<ApiResponse<NewsDto>> CreateAsync(
        CreateNewsDto dto,
        int userId,
        bool isAdmin)
    {
        var category =
            await _categoryRepository.GetByIdAsync(
                dto.CategoryId);

        if (category is null)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "دسته بندی پیدا نشد");
        }

        if (dto.CityId.HasValue)
        {
            var city =
                await _cityRepository.GetByIdAsync(
                    dto.CityId.Value);

            if (city is null)
            {
                return ApiResponse<NewsDto>.Failure(
                    ApiErrorCode.NotFound,
                    "شهر پیدا نشد");
            }
        }

        // دریافت و اعتبارسنجی Tag ها
        var tagIds =
            dto.TagIds
                .Distinct()
                .ToList();

        var tags =
            await _tagRepository.GetByIdsAsync(tagIds);

        if (tags.Count != tagIds.Count)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "یکی از تگ‌های انتخاب شده پیدا نشد");
        }

        var slug =
            _slugService.Generate(dto.Title);

        if (await _newsRepository.ExistsBySlugAsync(slug))
        {
            slug =
                await _slugService.GenerateUniqueAsync(slug);
        }

        var savedImages = new List<NewsImage>();

        for (int i = 0; i < dto.ImageFiles.Count; i++)
        {
            var file = dto.ImageFiles[i];

            var isValid =
                await _imageValidator.IsValidAsync(file);

            if (!isValid)
            {
                return ApiResponse<NewsDto>.Failure(
                    ApiErrorCode.BadRequest,
                    $"تصویر شماره {i + 1} معتبر نیست");
            }

            var imagePath =
                await _fileService.SaveFileAsync(
                    file,
                    "news");

            savedImages.Add(
                new NewsImage(
                    0,
                    imagePath,
                    i));
        }

        var news = new NewsEntity
        {
            Title = dto.Title,
            Slug = slug,
            Content = dto.Content,
            CategoryId = dto.CategoryId,
            CityId = dto.CityId,
            WriterId = userId,
            PublicationDate = DateTime.UtcNow,
            Status = isAdmin
                ? NewsStatus.Published
                : NewsStatus.PendingReview
        };

        // اتصال Tag ها به News
        foreach (var tag in tags)
        {
            news.NewsTags.Add(
                new NewsTag
                {
                    TagId = tag.Id
                });
        }

        await _newsRepository.AddAsync(news);

        foreach (var image in savedImages)
        {
            news.NewsImages.Add(image);
        }

        await _unitOfWork.CommitAsync();

        return ApiResponse<NewsDto>.Success(
            MapToDto(news));
    }

 
public async Task<ApiResponse<NewsDto>> UpdateAsync(
    int id,
    UpdateNewsDto dto,
    int userId,
    bool isAdmin)
    {
        var news =
            await _newsRepository.GetByIdAsync(id);

        if (news is null)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر پیدا نشد");
        }

        if (!isAdmin && news.WriterId != userId)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.Forbidden,
                "شما اجازه ویرایش این خبر را ندارید");
        }

        var category =
            await _categoryRepository.GetByIdAsync(
                dto.CategoryId);

        if (category is null)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "دسته بندی پیدا نشد");
        }

        if (dto.CityId.HasValue)
        {
            var city =
                await _cityRepository.GetByIdAsync(
                    dto.CityId.Value);

            if (city is null)
            {
                return ApiResponse<NewsDto>.Failure(
                    ApiErrorCode.NotFound,
                    "شهر پیدا نشد");
            }
        }

        var tagIds =
            dto.TagIds
                .Distinct()
                .ToList();

        var tags =
            await _tagRepository.GetByIdsAsync(tagIds);

        if (tags.Count != tagIds.Count)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "یکی از تگ‌های انتخاب شده پیدا نشد");
        }

        var newImages = new List<NewsImage>();

        if (dto.ImageFiles.Count > 0)
        {
            for (int i = 0; i < dto.ImageFiles.Count; i++)
            {
                var file = dto.ImageFiles[i];

                var isValid =
                    await _imageValidator.IsValidAsync(file);

                if (!isValid)
                {
                    return ApiResponse<NewsDto>.Failure(
                        ApiErrorCode.BadRequest,
                        $"تصویر شماره {i + 1} معتبر نیست");
                }

                var imagePath =
                    await _fileService.SaveFileAsync(
                        file,
                        "news");

                newImages.Add(
                    new NewsImage(
                        0,
                        imagePath,
                        i));
            }
        }

        var oldImagePaths =
            news.NewsImages
                .Select(image => image.ImagePath)
                .ToList();

        news.Update(
            dto.Title,
            dto.Content,
            dto.CategoryId,
            dto.CityId);

        news.NewsTags.Clear();

        foreach (var tag in tags)
        {
            news.NewsTags.Add(
                new NewsTag
                {
                    TagId = tag.Id
                });
        }

        if (newImages.Count > 0)
        {
            foreach (var oldImage in news.NewsImages.ToList())
            {
                _newsImageRepository.Remove(oldImage);
            }

            news.NewsImages.Clear();

            foreach (var image in newImages)
            {
                news.NewsImages.Add(image);
            }
        }

        if (!isAdmin)
        {
            news.ChangeStatus(
                NewsStatus.PendingReview);
        }

        await _unitOfWork.CommitAsync();

        if (newImages.Count > 0)
        {
            foreach (var oldImagePath in oldImagePaths)
            {
                if (!string.IsNullOrWhiteSpace(oldImagePath))
                {
                    await _fileService.DeleteFileAsync(
                        oldImagePath);
                }
            }
        }

        return ApiResponse<NewsDto>.Success(
            MapToDto(news));
    }
public async Task<ApiResponse<bool>> DeleteAsync(
    int id,
    int userId,
    bool isAdmin)
    {
        var news =
            await _newsRepository.GetByIdAsync(id);

        if (news is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "خبر پیدا نشد");
        }

        if (!isAdmin && news.WriterId != userId)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.Forbidden,
                "شما اجازه حذف این خبر را ندارید");
        }

        var imagePaths =
            news.NewsImages
                .Select(image => image.ImagePath)
                .ToList();

        _newsRepository.Remove(news);

        await _unitOfWork.CommitAsync();

        foreach (var imagePath in imagePaths)
        {
            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                await _fileService.DeleteFileAsync(
                    imagePath);
            }
        }

        return ApiResponse<bool>.Success(
            true,
            "خبر با موفقیت حذف شد");
    }



    public async Task<ApiResponse<NewsDto>> ChangeStatusAsync(
        int id,
        ChangeNewsStatusDto dto)
    {
        var news =
            await _newsRepository.GetByIdAsync(id);

        if (news is null)
        {
            return ApiResponse<NewsDto>.Failure(
                ApiErrorCode.NotFound,
                "خبر پیدا نشد");
        }

        news.ChangeStatus(
            dto.Status);

        await _unitOfWork.CommitAsync();

        return ApiResponse<NewsDto>.Success(
            MapToDto(news),
            "وضعیت خبر با موفقیت تغییر کرد");
    }

    private static NewsDto MapToDto(
        NewsEntity news)
    {
        return new NewsDto
        {
            Id = news.Id,
            Title = news.Title,
            Slug = news.Slug,
            Content = news.Content,
            ImagePath = news.ImagePath,
            PublicationDate = news.PublicationDate,
            UpdatedAt = news.UpdatedAt,
            Status = news.Status.ToString(),

            CityId = news.CityId,
            CityName = news.City?.Name,

            CategoryId = news.CategoryId,
            CategoryName = news.Category?.Name,

            WriterId = news.WriterId,
            WriterName = news.Writer?.Username,

            Tags = news.NewsTags
                .Select(nt => new TagDto
                {
                    Id = nt.Tag.Id,
                    Name = nt.Tag.Name,
                    Slug = nt.Tag.Slug
                })
                .ToList(),

            Images = news.NewsImages
                .Select(image => new NewsImageDto
                {
                    Id = image.Id,
                    ImagePath = image.ImagePath,
                    DisplayOrder = image.DisplayOrder
                })
                .OrderBy(image => image.DisplayOrder)
                .ThenBy(image => image.Id)
                .ToList(),
        };
    }


}
