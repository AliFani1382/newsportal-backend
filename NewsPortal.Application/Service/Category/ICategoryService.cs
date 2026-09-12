using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Service.Category
{
    public interface ICategoryService
    {
        Task<ApiResponse<IReadOnlyList<CategoryDto>>> GetAllAsync();
        Task<ApiResponse<CategoryDto>> GetByIdAsync(int id);

        Task<ApiResponse<CategoryDto>> GetBySlugAsync(string slug);

        Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryDto dto);
        Task<ApiResponse<bool>> UpdateAsync (int id,UpdateCategoryDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);

      


    }
}
