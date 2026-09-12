using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.API.Extensions;
using NewsPortal.Application.DTOs.Newsletter;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.API.Controllers;

[ApiController]
[Route("api/newsletter")]
[AllowAnonymous]
public class NewsletterController : ControllerBase
{
    private readonly INewsletterService _newsletterService;

    public NewsletterController(
        INewsletterService newsletterService)
    {
        _newsletterService = newsletterService;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe(
        [FromBody] SubscribeNewsletterDto dto)
    {
        var response =
            await _newsletterService.SubscribeAsync(dto);

        return response.ToHttpResult();
    }

    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe(
        [FromBody] UnsubscribeNewsletterDto dto)
    {
        var response =
            await _newsletterService.UnsubscribeAsync(dto);

        return response.ToHttpResult();
    }
}