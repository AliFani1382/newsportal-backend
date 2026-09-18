using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.EmailVerification;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class EmailVerificationController : ControllerBase
{
    private readonly IEmailVerificationService _emailVerificationService;

    public EmailVerificationController(
        IEmailVerificationService emailVerificationService)
    {
        _emailVerificationService = emailVerificationService;
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail(
        [FromBody] VerifyEmailDto dto)
    {
        var response =
            await _emailVerificationService.VerifyEmailAsync(dto);

        return response.ToHttpResult();
    }
    [HttpPost("resend-verification")]
    [EnableRateLimiting("ResendVerificationRateLimit")]
    public async Task<IActionResult> ResendVerification(
    [FromBody] ResendVerificationDto dto)
    {
        var response =
            await _emailVerificationService.ResendVerificationAsync(dto);

        return response.ToHttpResult();
    }
}