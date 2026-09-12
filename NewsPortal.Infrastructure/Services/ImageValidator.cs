using Microsoft.AspNetCore.Http;
using NewsPortal.Application.Common;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.Infrastructure.Services;

public sealed class ImageValidator : IImageValidator
{
    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

    private static readonly HashSet<string> AllowedContentTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

    public Task<bool> IsValidAsync(IFormFile file)
    {
        if (file is null)
            return Task.FromResult(false);

        if (file.Length <= 0)
            return Task.FromResult(false);

        if (file.Length > FileUploadLimits.MaxImageSize)
            return Task.FromResult(false);

        var extension =
            Path.GetExtension(file.FileName);

        if (string.IsNullOrWhiteSpace(extension))
            return Task.FromResult(false);

        if (!AllowedExtensions.Contains(extension))
            return Task.FromResult(false);

        if (string.IsNullOrWhiteSpace(file.ContentType))
            return Task.FromResult(false);

        if (!AllowedContentTypes.Contains(file.ContentType))
            return Task.FromResult(false);

        return Task.FromResult(true);
    }
}