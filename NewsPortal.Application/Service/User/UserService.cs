using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Profile;
using NewsPortal.Application.DTOs.User;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;
using UserEntity = NewsPortal.Domain.Entities.User;

namespace NewsPortal.Application.Service.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<UserProfileDto>> GetByIdAsync(int id)
        {
            var user =
                await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                return ApiResponse<UserProfileDto>.Failure(ApiErrorCode.NotFound, ".کاربر مورد نظر پیدا نشد");
            }
            return ApiResponse<UserProfileDto>
                .Success(MapToDto(user));
        }

        public async Task<ApiResponse<UserProfileDto>> UpdateAsync(int id, UpdateProfileDto dto)
        {
            var user =
                 await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                return ApiResponse<UserProfileDto>.Failure(
                    ApiErrorCode.NotFound,
                    "کاربر پیدا نشد");
            }

            var emailExists =
                await _userRepository.ExistsByEmailAsync(
                    dto.Email,
                    id);
            if (emailExists)
            {
                return ApiResponse<UserProfileDto>.Failure(
                    ApiErrorCode.Conflict,
                    ".این ایمیل قبلا توسط کاربر دیگری استفاده شده است");
            }

            user.UpdateProfile(
                dto.FullName,
                dto.Email);

            _userRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return ApiResponse<UserProfileDto>.Success(MapToDto(user));
        }
        public async Task<ApiResponse<bool>> ChangePasswordAsync(
            int id,
            ChangedPasswordDto dto)
        {
            var user =
                 await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                return ApiResponse<bool>.Failure(
                    ApiErrorCode.NotFound,
                    ".کاربر مورد نظر پیدا نشد");
            }
            var currentPasswordIsValid =
               _passwordHasher.Verify(
                   dto.CurrentPassword,
                   user.PasswordHash);

            if (!currentPasswordIsValid)
            {
                return ApiResponse<bool>.Failure(
                    ApiErrorCode.BadRequest,
                    ".رمز عبور فعلی صحیح نیست");
            }

            var newPasswordHash =
                _passwordHasher.Hash(dto.NewPassword);

            user.ChangePassword(
                newPasswordHash);

            _userRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return ApiResponse<bool>
                .Success(
                true,
                ".رمز عبور با موفقیت تغییر کرد");

             }

        public async Task<ApiResponse<IReadOnlyList<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var dtos = users
                .Select(MapToUserDto)
                .ToList();

            return ApiResponse<IReadOnlyList<UserDto>>.Success(dtos);
        }

        public async Task<ApiResponse<UserDto>> ChangeRoleAsync(
            int id,
            ChangeUserRoleDto dto)
        {
            var user =
                await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                return ApiResponse<UserDto>.Failure(
                    ApiErrorCode.NotFound,
                    ".کاربر مورد نظر پیدا نشد");
            }

            var role =
                await _roleRepository.GetNameAsync(dto.Role);

            if (role is null)
            {
                return ApiResponse<UserDto>.Failure(
                    ApiErrorCode.ValidationError,
                    ".نقش وارد شده معتبر نیست");
            }

            user.ChangeRole(role.Id);

            _userRepository.Update(user);
            await _unitOfWork.CommitAsync();

            var updatedDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = role.Name,
                IsActive = user.IsActive
            };

            return ApiResponse<UserDto>.Success(
                updatedDto,
                ".نقش کاربر با موفقیت تغییر کرد");
        }

        private static UserDto MapToUserDto(UserEntity user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role?.Name ?? string.Empty,
                IsActive = user.IsActive
            };
        }

        private static UserProfileDto MapToDto(UserEntity user)
        {
            return new UserProfileDto
            {
                Id = user.Id,
                UserName = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role?.Name ?? string.Empty,
                IsActive = user.IsActive
            };
        }

    }
            
}
    

         

        