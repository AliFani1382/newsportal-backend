using Microsoft.AspNetCore.Http;

namespace NewsPortal.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task DeleteFileAsync(string? filepath);
    }
}
