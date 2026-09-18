using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.PasswordReset;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class PasswordResetController : ControllerBase
{
    private readonly IPasswordResetService _passwordResetService;

    public PasswordResetController(
        IPasswordResetService passwordResetService)
    {
        _passwordResetService = passwordResetService;
    }

    [HttpPost("forgot-password")]
    [EnableRateLimiting("ForgotPasswordRateLimit")]

    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordDto dto)
    {
        var response =
            await _passwordResetService.ForgotPasswordAsync(dto);

        return response.ToHttpResult();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordDto dto)
    {
        var response =
            await _passwordResetService.ResetPasswordAsync(dto);

        return response.ToHttpResult();
    }
}