using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Helpers;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.City;
using NewsPortal.Application.Repositories;
using CityEntity = NewsPortal.Domain.Entities.City;

namespace NewsPortal.Application.Service.city;

public sealed class CityService : ICityService
{

    private readonly ICityRepository _cityRepository;
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CityService(
    ICityRepository cityRepository,
    INewsRepository newsRepository,
    IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ApiResponse<IReadOnlyList<CityDto>>> GetAllAsync()
    {

        var cities =
            await _cityRepository.GetAllAsync();

        var result =
            cities
            .Select(MapToDto)
            .ToList();

        return ApiResponse<IReadOnlyList<CityDto>>
            .Success(result);

    }
    public async Task<ApiResponse<CityDto>> GetByIdAsync(
        int id)
    {

        var city =
            await _cityRepository.GetByIdAsync(id);

        if (city is null)
        {
            return ApiResponse<CityDto>.Failure(
                ApiErrorCode.NotFound,
                "شهر پیدا نشد");
        }

        return ApiResponse<CityDto>
            .Success(MapToDto(city));

    }
    public async Task<ApiResponse<CityDto>> GetBySlugAsync(
        string slug)
    {

        var city =
            await _cityRepository.GetBySlugAsync(slug);

        if (city is null)
        {
            return ApiResponse<CityDto>.Failure(
                ApiErrorCode.NotFound,
                "شهر پیدا نشد");
        }

        return ApiResponse<CityDto>
            .Success(MapToDto(city));
    }
    public async Task<ApiResponse<CityDto>> CreateAsync(
        CreateCityDto dto)
    {

        var slug =
            await GenerateUniqueSlugAsync(
                string.IsNullOrWhiteSpace(dto.Slug)
                ? dto.Name
                : dto.Slug);

        var city = new CityEntity
        {
            Name = dto.Name,
            Slug = slug
        };

        await _cityRepository.AddAsync(city);

        await _unitOfWork.CommitAsync();

        return ApiResponse<CityDto>
            .Success(MapToDto(city));

    }
    public async Task<ApiResponse<CityDto>> UpdateAsync(
        int id,
        UpdateCityDto dto)
    {

        var city =
            await _cityRepository.GetByIdAsync(id);

        if (city is null)
        {
            return ApiResponse<CityDto>.Failure(
                ApiErrorCode.NotFound,
                "شهر پیدا نشد");
        }

        var slug =
            await GenerateUniqueSlugAsync(
                string.IsNullOrWhiteSpace(dto.Slug)
                ? dto.Name
                : dto.Slug,
                id);

        city.Name = dto.Name;
        city.Slug = slug;

        _cityRepository.Update(city);

        await _unitOfWork.CommitAsync();

        return ApiResponse<CityDto>
            .Success(MapToDto(city));

    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var city =
            await _cityRepository.GetByIdAsync(id);

        if (city is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "شهر مورد نظر پیدا نشد");
        }

        var hasNews =
            await _newsRepository.AnyAsync(
                x => x.CityId == id);

        if (hasNews)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.Conflict,
                "این شهر دارای خبر است و قابل حذف نیست");
        }

        _cityRepository.Remove(city);

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "شهر با موفقیت حذف شد");
    }
    
    private async Task<string> GenerateUniqueSlugAsync(
        string value,
        int? excludeId = null)
    {

        var slug =
            SlugHelper.GenerateSlug(value);

        if (string.IsNullOrWhiteSpace(slug))
        {
            slug = "city";
        }

        var original = slug;

        var counter = 1;

        while (await _cityRepository
            .ExistsBySlugAsync(slug, excludeId))
        {

            slug =
            $"{original}-{counter}";

            counter++;
        }
        return slug;
    }

    private static CityDto MapToDto(
        CityEntity city)
    {
        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            Slug = city.Slug
        };
    }

}