using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.Reactions;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/news/{newsId:int}/reaction")]
[Authorize]
public class ReactionsController : ControllerBase
{
    private readonly IReactionService _reactionService;

    public ReactionsController(
        IReactionService reactionService)
    {
        _reactionService = reactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetReaction(
        int newsId)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _reactionService.GetReactionAsync(
                newsId,
                userId);

        return response.ToHttpResult();
    }

    [HttpPut]
    public async Task<IActionResult> SetReaction(
        int newsId,
        [FromBody] SetReactionDto dto)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var response =
            await _reactionService.SetReactionAsync(
                newsId,
                userId,
                dto.Type);

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
            errorResult = Unauthorized();

            return false;
        }

        return true;
    }
}