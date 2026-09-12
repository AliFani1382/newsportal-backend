namespace NewsPortal.Application.Common;

public static class FileUploadLimits
{
    public const long MaxImageSize = 2 * 1024 * 1024;

    public const long MultipartOverhead = 512 * 1024;

    public const long MaxMultipartRequestSize =
        MaxImageSize + MultipartOverhead;

    public const string UploadsRootFolder = "uploads";

    public const string MaxImageSizeMessage =
        "حجم تصویر نباید بیشتر از ۲ مگابایت باشد.";

    public const string AllowedImageExtensionsMessage =
        "فرمت تصویر مجاز نیست. پسوندهای مجاز: .jpg, .jpeg, .png, .webp";

    public static readonly string[] AllowedImageExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };
}
