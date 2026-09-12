using Microsoft.AspNetCore.Http;

namespace NewsPortal.Application.Interfaces;

public interface IImageValidator
{
    Task<bool> IsValidAsync(IFormFile file);
}