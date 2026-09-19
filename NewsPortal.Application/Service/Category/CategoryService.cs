using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Helpers;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Category;
using NewsPortal.Application.Repositories;
using CategoryEntity = NewsPortal.Domain.Entities.Category;

namespace NewsPortal.Application.Service.Category;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        ICategoryRepository categoryRepository,
        INewsRepository newsRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IReadOnlyList<CategoryDto>>> GetAllAsync()
    {
        var categories =
            await _categoryRepository.GetAllAsync();

        var result =
            categories
            .Select(MapToDto)
            .ToList();

        return ApiResponse<IReadOnlyList<CategoryDto>>
            .Success(result);
    }

    public async Task<ApiResponse<CategoryDto>> GetByIdAsync(int id)
    {
        var category =
            await _categoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            return ApiResponse<CategoryDto>.Failure(
                ApiErrorCode.NotFound,
                "دسته بندی پیدا نشد");
        }

        return ApiResponse<CategoryDto>
            .Success(MapToDto(category));
    }

    public async Task<ApiResponse<CategoryDto>> GetBySlugAsync(
        string slug)
    {
        var category =
            await _categoryRepository.GetBySlugAsync(slug);

        if (category is null)
        {
            return ApiResponse<CategoryDto>.Failure(
                ApiErrorCode.NotFound,
                "دسته بندی پیدا نشد");
        }

        return ApiResponse<CategoryDto>
            .Success(MapToDto(category));
    }

    public async Task<ApiResponse<CategoryDto>> CreateAsync(
        CreateCategoryDto dto)
    {

        var slug =
            await GenerateUniqueSlugAsync(
                string.IsNullOrWhiteSpace(dto.Slug)
                ? dto.Name
                : dto.Slug);

        var category = new CategoryEntity
        {
            Name = dto.Name,
            Slug = slug
        };

        await _categoryRepository.AddAsync(category);

        await _unitOfWork.CommitAsync();

        return ApiResponse<CategoryDto>
            .Success(MapToDto(category));
    }

    public async Task<ApiResponse<bool>> UpdateAsync(
        int id,
        UpdateCategoryDto dto)
    {

        var category =
            await _categoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "دسته بندی پیدا نشد");
        }

        var slug =
            await GenerateUniqueSlugAsync(
                string.IsNullOrWhiteSpace(dto.Slug)
                ? dto.Name
                : dto.Slug,
                id);

        category.Name = dto.Name;
        category.Slug = slug;

        _categoryRepository.Update(category);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>
            .Success(
                true,
                "دسته بندی با موفقیت ویرایش شد");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(
        int id)
    {

        var category =
            await _categoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "دسته بندی پیدا نشد");
        }

        var hasNews =
            await _newsRepository.AnyAsync(
                x => x.CategoryId == id);

        if (hasNews)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.Conflict,
                "این دسته بندی دارای خبر است و قابل حذف نیست");
        }

        _categoryRepository.Remove(category);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>
            .Success(
                true,
                "دسته بندی حذف شد");
    }

    private async Task<string> GenerateUniqueSlugAsync(
        string value,
        int? excludeId = null)
    {

        var slug =
            SlugHelper.GenerateSlug(value);

        if (string.IsNullOrWhiteSpace(slug))
        {
            slug = "category";
        }

        var originalSlug = slug;

        var counter = 1;

        while (true)
        {

            var exists =
                await _categoryRepository
                .ExistsBySlugAsync(
                    slug,
                    excludeId);

            if (!exists)
            {
                return slug;
            }

            slug =
                $"{originalSlug}-{counter}";

            counter++;
        }
    }

    private CategoryDto MapToDto(
        CategoryEntity category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug
        };
    }

}