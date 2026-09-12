using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.EmailVerification;

namespace NewsPortal.Application.Interfaces;

public interface IEmailVerificationService
{
    Task<ApiResponse<bool>> VerifyEmailAsync(VerifyEmailDto dto);

    Task<ApiResponse<bool>> ResendVerificationAsync(
        ResendVerificationDto dto);
}