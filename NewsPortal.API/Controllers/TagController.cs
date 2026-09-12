using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.Tags;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/tags")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var response = await _tagService.GetAllAsync();

        return response.ToHttpResult();
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _tagService.GetByIdAsync(id);

        return response.ToHttpResult();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateTagDto dto)
    {
        var response = await _tagService.CreateAsync(dto);

        return response.ToHttpResult();
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateTagDto dto)
    {
        var response = await _tagService.UpdateAsync(id, dto);

        return response.ToHttpResult();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _tagService.DeleteAsync(id);

        return response.ToHttpResult();
    }
}