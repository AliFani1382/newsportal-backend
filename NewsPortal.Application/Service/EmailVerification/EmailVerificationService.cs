using System.Security.Cryptography;
using NewsPortal.Application.Common;
using NewsPortal.Application.Common.Interfaces;
using NewsPortal.Application.DTOs.EmailVerification;
using NewsPortal.Application.Interfaces;
using NewsPortal.Application.Repositories;

namespace NewsPortal.Application.Service.EmailVerification;

public sealed class EmailVerificationService : IEmailVerificationService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationTokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public EmailVerificationService(
        IUserRepository userRepository,
        IEmailVerificationTokenRepository tokenRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<ApiResponse<VerifyEmailResponseDto>> VerifyEmailAsync(
        VerifyEmailDto dto)
    {
        var verificationToken =
            await _tokenRepository.GetValidTokenAsync(dto.Token);

        if (verificationToken is null)
        {
            return ApiResponse<VerifyEmailResponseDto>.Failure(
                ApiErrorCode.ValidationError,
                "توکن تأیید ایمیل نامعتبر یا منقضی شده است.");
        }

        var user =
            await _userRepository.GetByIdAsync(
                verificationToken.UserId);

        if (user is null)
        {
            return ApiResponse<VerifyEmailResponseDto>.Failure(
                ApiErrorCode.NotFound,
                "کاربر مورد نظر پیدا نشد.");
        }

        user.VerifyEmail();

        verificationToken.MarkAsUsed();

        await _unitOfWork.CommitAsync();

        return ApiResponse<VerifyEmailResponseDto>.Success(
            new VerifyEmailResponseDto
            {
                IsVerified = true
            },
            "ایمیل با موفقیت تأیید شد.");
    }

    public async Task<ApiResponse<bool>> ResendVerificationAsync(
        ResendVerificationDto dto)
    {
        var user =
            await _userRepository.GetByEmailAsync(dto.Email);

        if (user is null || user.IsEmailVerified)
        {
            return ApiResponse<bool>.Success(
                true,
                "اگر ایمیل ثبت شده باشد، لینک تأیید ارسال خواهد شد.");
        }

        var activeTokens =
            await _tokenRepository.GetValidTokensByUserIdAsync(user.Id);

        foreach (var token in activeTokens)
        {
            token.MarkAsUsed();
        }

        var verificationToken =
            Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(32));

        var expiresAt =
            DateTime.UtcNow.AddHours(24);

        var newToken =
            new Domain.Entities.EmailVerificationToken(
                user.Id,
                verificationToken,
                expiresAt);

        await _tokenRepository.AddAsync(newToken);

        await _unitOfWork.CommitAsync();

        await _emailService.SendEmailVerificationEmailAsync(
            user.Email,
            verificationToken);

        return ApiResponse<bool>.Success(
            true,
            "اگر ایمیل ثبت شده باشد، لینک تأیید ارسال خواهد شد.");
    }
}