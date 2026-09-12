using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class BookmarksController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarksController(
        IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    [HttpGet("bookmarks")]
    public async Task<IActionResult> GetMyBookmarks()
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _bookmarkService.GetMyBookmarksAsync(
                userId);

        return response.ToHttpResult();
    }

    [HttpGet("news/{newsId:int}/bookmark")]
    public async Task<IActionResult> GetStatus(
        int newsId)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _bookmarkService.GetStatusAsync(
                newsId,
                userId);

        return response.ToHttpResult();
    }

    [HttpPost("news/{newsId:int}/bookmark")]
    public async Task<IActionResult> Add(
        int newsId)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _bookmarkService.AddAsync(
                newsId,
                userId);

        return response.ToHttpResult();
    }

    [HttpDelete("news/{newsId:int}/bookmark")]
    public async Task<IActionResult> Remove(
        int newsId)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _bookmarkService.RemoveAsync(
                newsId,
                userId);

        return response.ToHttpResult();
    }

    private bool TryGetUserId(
        out int userId,
        out IActionResult? errorResult)
    {
        userId = 0;
        errorResult = null;

        var userIdClaim =
            User.FindFirst(
                ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(
                userIdClaim.Value,
                out userId))
        {
            errorResult = Unauthorized();
            return false;
        }

        return true;
    }
}