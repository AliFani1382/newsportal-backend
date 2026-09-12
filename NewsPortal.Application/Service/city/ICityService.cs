using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Service.city
{
    public interface ICityService
    {

        Task<ApiResponse<IReadOnlyList<CityDto>>> GetAllAsync();
        Task<ApiResponse<CityDto>> GetByIdAsync(int id);
        Task<ApiResponse<CityDto>> GetBySlugAsync(string slug);

        Task<ApiResponse<CityDto>> CreateAsync(CreateCityDto dto);

        Task<ApiResponse<CityDto>> UpdateAsync(int id, UpdateCityDto dto);

        Task<ApiResponse<bool>> DeleteAsync(int id);

    }

}
