using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.User;
using NewsPortal.Application.Service.User;
using NewsPortal.Domain.Constants;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = RoleNames.Admin)]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _userService.GetAllUsersAsync();
        return response.ToHttpResult();
    }

    [HttpPut("{id:int}/role")]
    public async Task<IActionResult> ChangeRole(
        int id,
        [FromBody] ChangeUserRoleDto dto)
    {
        var response = await _userService.ChangeRoleAsync(id, dto);
        return response.ToHttpResult();
    }
}
