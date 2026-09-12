using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.PasswordReset;

namespace NewsPortal.Application.Interfaces;

public interface IPasswordResetService
{
    Task<ApiResponse<bool>> ForgotPasswordAsync(
        ForgotPasswordDto dto);

    Task<ApiResponse<bool>> ResetPasswordAsync(
        ResetPasswordDto dto);
}