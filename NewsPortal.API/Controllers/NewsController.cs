using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.News;
using NewsPortal.Application.Service.News;
using NewsPortal.Domain.Constants;
using System.Security.Claims;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? categoryId = null,
        [FromQuery] int? cityId = null,
        [FromQuery] string? search = null)
    {
        var (_, isAdmin) = GetOptionalIdentity();

        var response =
            await _newsService.GetPagedAsync(
                pageNumber,
                pageSize,
                categoryId,
                cityId,
                search,
                isAdmin);

        return response.ToHttpResult();
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var (userId, isAdmin) = GetOptionalIdentity();

        var response =
            await _newsService.GetByIdAsync(id, userId, isAdmin);

        return response.ToHttpResult();
    }

    [HttpPost("{id:int}/view")]
    [AllowAnonymous]
    public async Task<IActionResult> IncrementView(
    int id)
    {
        var response =
            await _newsService.IncrementViewCountAsync(
                id);

        return response.ToHttpResult();
    }

    [HttpGet("by-slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var (userId, isAdmin) = GetOptionalIdentity();

        var response =
            await _newsService.GetBySlugAsync(slug, userId, isAdmin);

        return response.ToHttpResult();
    }

    [HttpGet("{id:int}/related")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRelated(
    int id,
    [FromQuery] int count = 5)
    {
        var response =
            await _newsService.GetRelatedAsync(
                id,
                count);

        return response.ToHttpResult();
    }

    [HttpGet("popular")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPopular(
    [FromQuery] int count = 5)
    {
        var response =
            await _newsService.GetPopularAsync(
                count);

        return response.ToHttpResult();
    }

    [HttpGet("featured")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeatured(
    [FromQuery] int count = 5)
    {
        var response =
            await _newsService.GetFeaturedAsync(
                count);

        return response.ToHttpResult();
    }

    [HttpPut("{id:int}/featured")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SetFeatured(
    int id,
    [FromQuery] bool isFeatured)
    {
        var response =
            await _newsService.SetFeaturedAsync(
                id,
                isFeatured);

        return response.ToHttpResult();
    }

    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateNewsDto dto)
    {
        if (!TryGetUserId(
                out var userId,
                out var errorResult))
        {
            return errorResult!;
        }

        var isAdmin = User.IsInRole(RoleNames.Admin);

        var response =
            await _newsService.CreateAsync(
                dto,
                userId,
                isAdmin);

        return response.ToHttpResult(
            onSuccess: () =>
                CreatedAtAction(
                    nameof(GetById),
                    new { id = response.Data!.Id },
                    response));
    }
[HttpPut("{id:int}")]
[Authorize]
[Consumes("multipart/form-data")]
public async Task<IActionResult> Update(
    int id,
    [FromForm] UpdateNewsDto dto)
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
            await _newsService.UpdateAsync(
                id,
                dto,
                userId,
                isAdmin);

        return response.ToHttpResult();
    }
    

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
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
            await _newsService.DeleteAsync(
                id,
                userId,
                isAdmin);

        return response.ToHttpResult();
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> ChangeStatus(
        int id,
        [FromBody] ChangeNewsStatusDto dto)
    {
        var response =
            await _newsService.ChangeStatusAsync(id, dto);

        return response.ToHttpResult();
    }

    private (int? UserId, bool IsAdmin) GetOptionalIdentity()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return (null, false);
        }

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        int? userId =
            int.TryParse(userIdClaim, out var parsedId)
                ? parsedId
                : null;

        var isAdmin = User.IsInRole(RoleNames.Admin);

        return (userId, isAdmin);
    }

    private bool TryGetUserId(
        out int userId,
        out IActionResult? errorResult)
    {
        userId = 0;
        errorResult = null;

        var userIdClaim =
            User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim) ||
            !int.TryParse(
                userIdClaim,
                out userId))
        {
            errorResult =
                ApiResponse.Failure(
                    ApiErrorCode.Unauthorized,
                    "شناسه کاربر معتبر نیست یا منقضی شده است.")
                .ToHttpResult();

            return false;
        }

        return true;
    }
}