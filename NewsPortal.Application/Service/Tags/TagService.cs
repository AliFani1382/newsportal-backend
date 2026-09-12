using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Tags;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Service.Tags;

public sealed class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TagService(
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IReadOnlyList<TagDto>>> GetAllAsync()
    {
        var tags = await _tagRepository.GetAllAsync();

        var result = tags
            .Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                Slug = t.Slug
            })
            .ToList();

        return ApiResponse<IReadOnlyList<TagDto>>.Success(result);
    }

    public async Task<ApiResponse<TagDto>> GetByIdAsync(int id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);

        if (tag is null)
        {
            return ApiResponse<TagDto>.Failure(
                ApiErrorCode.NotFound,
                "برچسب مورد نظر پیدا نشد.");
        }

        var result = new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Slug = tag.Slug
        };

        return ApiResponse<TagDto>.Success(result);
    }

    public async Task<ApiResponse<TagDto>> CreateAsync(
        CreateTagDto dto)
    {
        var existingByName =
            await _tagRepository.GetByNameAsync(dto.Name);

        if (existingByName is not null)
        {
            return ApiResponse<TagDto>.Failure(
                ApiErrorCode.Conflict,
                "این نام برچسب قبلاً ثبت شده است.");
        }

        var existingBySlug =
            await _tagRepository.GetBySlugAsync(dto.Slug);

        if (existingBySlug is not null)
        {
            return ApiResponse<TagDto>.Failure(
                ApiErrorCode.Conflict,
                "این Slug قبلاً ثبت شده است.");
        }

        var tag = new Tag
        {
            Name = dto.Name,
            Slug = dto.Slug,
            CreatedDate = DateTime.UtcNow
        };

        await _tagRepository.AddAsync(tag);
        await _unitOfWork.CommitAsync();

        return ApiResponse<TagDto>.Success(
            new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Slug = tag.Slug
            },
            "برچسب با موفقیت ایجاد شد.");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(
        int id,
        UpdateTagDto dto)
    {
        var tag = await _tagRepository.GetByIdAsync(id);

        if (tag is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "برچسب مورد نظر پیدا نشد.");
        }

        var existingByName =
            await _tagRepository.GetByNameAsync(dto.Name);

        if (existingByName is not null &&
            existingByName.Id != id)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.Conflict,
                "این نام برچسب قبلاً ثبت شده است.");
        }

        var existingBySlug =
            await _tagRepository.GetBySlugAsync(dto.Slug);

        if (existingBySlug is not null &&
            existingBySlug.Id != id)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.Conflict,
                "این Slug قبلاً ثبت شده است.");
        }

        tag.Name = dto.Name;
        tag.Slug = dto.Slug;
        tag.UpdatedAt = DateTime.UtcNow;

        _tagRepository.Update(tag);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "برچسب با موفقیت ویرایش شد.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);

        if (tag is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "برچسب مورد نظر پیدا نشد.");
        }

        _tagRepository.Remove(tag);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "برچسب با موفقیت حذف شد.");
    }
}