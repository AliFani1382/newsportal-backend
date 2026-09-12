using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.Auth;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Constants;
using System.Security.Cryptography;
using NewsPortal.Domain.Entities;

using UserEntity = NewsPortal.Domain.Entities.User;

namespace NewsPortal.Application.Service.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService tokenService,
        IUnitOfWork unitOfWork,
        IRoleRepository roleRepository,
        IEmailVerificationTokenRepository emailVerificationTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _roleRepository = roleRepository;
        _emailVerificationTokenRepository = emailVerificationTokenRepository;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(
        RegisterDto dto)
    {

        var usernameExists =
            await _userRepository.ExistsByUsernameAsync(dto.Username);

        if (usernameExists)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                ApiErrorCode.Conflict,
                "نام کاربری قبلاً استفاده شده است.");
        }
       
        var emailExists =
            await _userRepository.ExistsByEmailAsync(dto.Email);

        if (emailExists)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                ApiErrorCode.Conflict,
                "ایمیل قبلاً استفاده شده است.");
        }
      

        var role = await _roleRepository.GetNameAsync(
            RoleNames.User);

        if (role is null)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                ApiErrorCode.NotFound,
                ".نقش کاربر پیدا نشد");
        }

        var passwordHash =
            _passwordHasher.Hash(dto.Password);

        var user = new UserEntity(
            dto.Username,
            dto.Email,
            passwordHash,
            dto.FullName,
            role.Id);
      

        await _userRepository.AddAsync(user);

        // ابتدا کاربر را ذخیره می‌کنیم تا UserId تولید شود.
        await _unitOfWork.CommitAsync();

      

        var verificationToken = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(32));

        var expiresAt = DateTime.UtcNow.AddHours(24);

        var emailVerificationToken = new EmailVerificationToken(
            user.Id,
            verificationToken,
            expiresAt);

        await _emailVerificationTokenRepository.AddAsync(
            emailVerificationToken);

        await _unitOfWork.CommitAsync();

        var token = _tokenService.GenerateToken(user);

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Token = token,
            IsEmailVerified = user.IsEmailVerified
        };

        return ApiResponse<AuthResponseDto>.Success(response);
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(
        LoginDto dto)
    {
        var user =
            await _userRepository.GetByUsernameOrEmailAsync(
                dto.UsernameOrEmail);

        if (user is null)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                ApiErrorCode.Unauthorized,
                "نام کاربری یا رمز عبور اشتباه است.");
        }

        if (!user.IsActive)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                ApiErrorCode.Forbidden,
                "حساب کاربری غیرفعال است.");
        }

        var passwordValid =
            _passwordHasher.Verify(
                dto.Password,
                user.PasswordHash);

        if (!passwordValid)
        {
            return ApiResponse<AuthResponseDto>.Failure(
                ApiErrorCode.Unauthorized,
                "نام کاربری یا رمز عبور اشتباه است.");
        }

        var token = _tokenService.GenerateToken(user);

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Token = token,
            IsEmailVerified = user.IsEmailVerified
        };

        return ApiResponse<AuthResponseDto>.Success(response);
    }
}