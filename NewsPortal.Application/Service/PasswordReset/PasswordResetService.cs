using System.Security.Cryptography;
using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.PasswordReset;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;
using NewsPortal.Domain.Entities;

namespace NewsPortal.Application.Service.PasswordReset;

public sealed class PasswordResetService : IPasswordResetService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public PasswordResetService(
        IUserRepository userRepository,
        IPasswordResetTokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<ApiResponse<bool>> ForgotPasswordAsync(
        ForgotPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user is null)
        {
            return ApiResponse<bool>.Success(
                true,
                "اگر ایمیل ثبت شده باشد، لینک بازیابی ارسال خواهد شد.");
        }

        var token = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(32));

        var expiresAt = DateTime.UtcNow.AddHours(1);

        var resetToken = new PasswordResetToken(
            user.Id,
            token,
            expiresAt);

        await _tokenRepository.AddAsync(resetToken);
        await _unitOfWork.CommitAsync();

        await _emailService.SendPasswordResetEmailAsync(
            user.Email,
            token);

        return ApiResponse<bool>.Success(
            true,
            "اگر ایمیل ثبت شده باشد، لینک بازیابی ارسال خواهد شد.");
    }

    public async Task<ApiResponse<bool>> ResetPasswordAsync(
        ResetPasswordDto dto)
    {
        var resetToken =
            await _tokenRepository.GetValidTokenAsync(dto.Token);

        if (resetToken is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.ValidationError,
                "توکن بازیابی نامعتبر یا منقضی شده است.");
        }

        var user =
            await _userRepository.GetByIdAsync(resetToken.UserId);

        if (user is null)
        {
            return ApiResponse<bool>.Failure(
                ApiErrorCode.NotFound,
                "کاربر مورد نظر پیدا نشد.");
        }

        var passwordHash =
            _passwordHasher.Hash(dto.NewPassword);

        user.ChangePassword(passwordHash);

        resetToken.MarkAsUsed();

        await _unitOfWork.CommitAsync();

        return ApiResponse<bool>.Success(
            true,
            "رمز عبور با موفقیت تغییر کرد.");
    }
}