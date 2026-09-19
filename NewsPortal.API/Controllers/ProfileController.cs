using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.Profile;
using NewsPortal.Application.Service.User;
using NewsPortal.API.Extensions;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    private bool TryGetUserId(
        out int userId,
        out IActionResult? errorResult)
    {
        userId = 0;
        errorResult = null;

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim) ||
            !int.TryParse(userIdClaim, out userId))
        {
            errorResult = ApiResponse.Failure(
                ApiErrorCode.Unauthorized,
                "شناسه کاربر معتبر نیست یا منقضی شده است."
            ).ToHttpResult();

            return false;
        }

        return true;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _userService.GetByIdAsync(userId);

        return response.ToHttpResult();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileDto dto)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _userService.UpdateAsync(
                userId,
                dto);

        return response.ToHttpResult();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangedPasswordDto dto)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _userService.ChangePasswordAsync(
                userId,
                dto);

        return response.ToHttpResult();
    }
}