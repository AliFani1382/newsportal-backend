using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.City;
using NewsPortal.API.Extensions; // فرض بر این است که ToHttpResult در این Namespace قرار دارد
using NewsPortal.Domain.Constants;
using NewsPortal.Application.Service.city;


namespace NewsPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var response = await _cityService.GetAllAsync();
        return response.ToHttpResult();
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _cityService.GetByIdAsync(id);
        return response.ToHttpResult();
    }

    [HttpGet("by-slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var response = await _cityService.GetBySlugAsync(slug);
        return response.ToHttpResult();
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateCityDto dto)
    {
        var response = await _cityService.CreateAsync(dto);

        return response.ToHttpResult(
            onSuccess: () => CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response)
        );
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCityDto dto)
    {
        var response = await _cityService.UpdateAsync(id, dto);
        return response.ToHttpResult();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _cityService.DeleteAsync(id);
        return response.ToHttpResult();
    }
}
