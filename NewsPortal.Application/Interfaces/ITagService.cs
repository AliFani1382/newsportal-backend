using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Tags;

namespace NewsPortal.Application.Interfaces;

public interface ITagService
{
    Task<ApiResponse<IReadOnlyList<TagDto>>> GetAllAsync();

    Task<ApiResponse<TagDto>> GetByIdAsync(int id);

    Task<ApiResponse<TagDto>> CreateAsync(CreateTagDto dto);

    Task<ApiResponse<bool>> UpdateAsync(
        int id,
        UpdateTagDto dto);

    Task<ApiResponse<bool>> DeleteAsync(int id);
}