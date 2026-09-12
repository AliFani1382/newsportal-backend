using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Profile;
using NewsPortal.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Service.User
{
    public interface IUserService
    {
        Task<ApiResponse<UserProfileDto>> GetByIdAsync(int id);
        Task<ApiResponse<UserProfileDto>> UpdateAsync(
            int id,
            UpdateProfileDto dto);
        Task<ApiResponse<bool>> ChangePasswordAsync(
            int id,
            ChangedPasswordDto dto);

        Task<ApiResponse<IReadOnlyList<UserDto>>> GetAllUsersAsync();

        Task<ApiResponse<UserDto>> ChangeRoleAsync(
            int id,
            ChangeUserRoleDto dto);

    }
}
