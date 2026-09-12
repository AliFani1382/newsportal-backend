using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NewsPortal.Application.Common;
using NewsPortal.Application.Interfaces;

namespace NewsPortal.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        if (file is null)
        {
            throw new ArgumentNullException(
                nameof(file),
                "فایل ارسال نشده است.");
        }

        if (file.Length == 0)
        {
            throw new ArgumentException(
                "فایل خالی است.",
                nameof(file));
        }

        if (file.Length > FileUploadLimits.MaxImageSize)
        {
            throw new ArgumentException(
                FileUploadLimits.MaxImageSizeMessage,
                nameof(file));
        }

        var webRoot = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRoot))
        {
            throw new InvalidOperationException(
                "WebRootPath is not configured.");
        }

        if (string.IsNullOrWhiteSpace(folder))
        {
            throw new ArgumentException(
                "نام پوشه معتبر نیست.",
                nameof(folder));
        }

        var safeFolder = Path.GetFileName(folder.Trim());

        if (string.IsNullOrWhiteSpace(safeFolder))
        {
            throw new ArgumentException(
                "نام پوشه معتبر نیست.",
                nameof(folder));
        }

        var extension = Path.GetExtension(file.FileName);

        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new ArgumentException(
                "فایل باید پسوند معتبر داشته باشد.",
                nameof(file));
        }

        extension = extension.ToLowerInvariant();

        if (!FileUploadLimits.AllowedImageExtensions.Contains(
                extension,
                StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                FileUploadLimits.AllowedImageExtensionsMessage,
                nameof(file));
        }

        if (!IsAllowedImage(file, extension))
        {
            throw new ArgumentException(
                "محتوای فایل تصویر معتبر نیست.",
                nameof(file));
        }

        var uploadsRoot = Path.Combine(
            webRoot,
            FileUploadLimits.UploadsRootFolder,
            safeFolder);

        Directory.CreateDirectory(uploadsRoot);

        var fileName = $"{Guid.NewGuid():N}{extension}";

        var filePath = Path.Combine(
            uploadsRoot,
            fileName);

        await using var stream = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None);

        await file.CopyToAsync(stream);

        return Path.Combine(
            FileUploadLimits.UploadsRootFolder,
            safeFolder,
            fileName)
            .Replace('\\', '/');
    }

    public async Task DeleteFileAsync(string? filepath)
    {
        if (string.IsNullOrWhiteSpace(filepath))
            return;

        var webRoot = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRoot))
        {
            throw new InvalidOperationException(
                "WebRootPath is not configured.");
        }

        var relativePath = filepath
            .Trim()
            .TrimStart('/', '\\')
            .Replace('/', Path.DirectorySeparatorChar);

        var uploadsRoot = Path.GetFullPath(
            Path.Combine(webRoot, FileUploadLimits.UploadsRootFolder));

        var fullFilePath = Path.GetFullPath(
            Path.Combine(webRoot, relativePath));

        var uploadsRootWithSeparator =
            uploadsRoot.EndsWith(Path.DirectorySeparatorChar)
                ? uploadsRoot
                : uploadsRoot + Path.DirectorySeparatorChar;

        if (!fullFilePath.StartsWith(
                uploadsRootWithSeparator,
                StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await Task.Run(() =>
        {
            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }
        });
    }

    private static bool IsAllowedImage(
        IFormFile file,
        string extension)
    {
        using var stream = file.OpenReadStream();

        Span<byte> header = stackalloc byte[12];

        var read = stream.Read(header);

        if (read < 8)
            return false;

        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" =>
                read >= 3 &&
                header[0] == 0xFF &&
                header[1] == 0xD8 &&
                header[2] == 0xFF,

            ".png" =>
                read >= 8 &&
                header[0] == 0x89 &&
                header[1] == 0x50 &&
                header[2] == 0x4E &&
                header[3] == 0x47 &&
                header[4] == 0x0D &&
                header[5] == 0x0A &&
                header[6] == 0x1A &&
                header[7] == 0x0A,

            ".webp" =>
                read >= 12 &&
                header[0] == 0x52 &&
                header[1] == 0x49 &&
                header[2] == 0x46 &&
                header[3] == 0x46 &&
                header[8] == 0x57 &&
                header[9] == 0x45 &&
                header[10] == 0x42 &&
                header[11] == 0x50,

            _ => false
        };
    }
}
