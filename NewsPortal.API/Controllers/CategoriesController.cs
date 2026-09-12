using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.Category;
using NewsPortal.Application.Service.Category;
using NewsPortal.Domain.Constants;

namespace NewsPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return response.ToHttpResult();
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            return response.ToHttpResult();
        }

        [AllowAnonymous]
        [HttpGet("by-slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var response = await _categoryService.GetBySlugAsync(slug);
            return response.ToHttpResult();
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var response = await _categoryService.CreateAsync(dto);

            return response.ToHttpResult(
                onSuccess: () => CreatedAtAction(
                    nameof(GetById),
                    new { id = response.Data?.Id },
                    response));
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            var response = await _categoryService.UpdateAsync(id, dto);
            return response.ToHttpResult();
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.DeleteAsync(id);
            return response.ToHttpResult();
        }
    }
}
