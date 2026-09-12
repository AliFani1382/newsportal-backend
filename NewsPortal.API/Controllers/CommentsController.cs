using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.Comments;
using NewsPortal.Application.Interfaces;
using NewsPortal.Domain.Constants;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(
        ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("news/{newsId:int}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByNewsId(
        int newsId)
    {
        var isAdmin =
            User.IsInRole(RoleNames.Admin);

        var response =
            await _commentService.GetByNewsIdAsync(
                newsId,
                isAdmin);

        return response.ToHttpResult();
    }

    [HttpPost("news/{newsId:int}/comments")]
    [Authorize]
    public async Task<IActionResult> Create(
        int newsId,
        [FromBody] CreateCommentDto dto)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _commentService.CreateAsync(
                newsId,
                dto,
                userId);

        return response.ToHttpResult();
    }

    [HttpGet("comments/pending")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> GetPending()
    {
        var response =
            await _commentService.GetPendingAsync();

        return response.ToHttpResult();
    }

    [HttpPut("comments/{id:int}/approve")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> Approve(
        int id)
    {
        var response =
            await _commentService.ApproveAsync(id);

        return response.ToHttpResult();
    }

    [HttpPut("comments/{id:int}/reject")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> Reject(
        int id)
    {
        var response =
            await _commentService.RejectAsync(id);

        return response.ToHttpResult();
    }

    [HttpDelete("comments/{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(
        int id)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var isAdmin =
            User.IsInRole(RoleNames.Admin);

        var response =
            await _commentService.DeleteAsync(
                id,
                userId,
                isAdmin);

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
                System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(
                userIdClaim.Value,
                out userId))
        {
            errorResult =
                Unauthorized();

            return false;
        }

        return true;
    }
}