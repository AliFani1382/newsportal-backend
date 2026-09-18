using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NewsPortal.Application.DTOs.Auth;
using NewsPortal.Application.Common;
using NewsPortal.API.Extensions;
using NewsPortal.Application.Service.Auth;

namespace NewsPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [EnableRateLimiting("RegisterRateLimit")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (!result.IsSuccess)
        {
            return result.ToHttpResult();
        }

        var successResponse = ApiResponse<AuthResponseDto>.Success(
            result.Data!,
            "ثبت‌نام با موفقیت انجام شد.");

        return successResponse.ToHttpResult();
    }

    [HttpPost("login")]
    [EnableRateLimiting("LoginRateLimit")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (!result.IsSuccess)
        {
            return result.ToHttpResult();
        }

        var successResponse = ApiResponse<AuthResponseDto>.Success(
            result.Data!,
            "ورود با موفقیت انجام شد.");

        return successResponse.ToHttpResult();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _authService.RefreshTokenAsync(dto);
        return result.ToHttpResult();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _authService.LogoutAsync(dto);
        return result.ToHttpResult();
    }
}

