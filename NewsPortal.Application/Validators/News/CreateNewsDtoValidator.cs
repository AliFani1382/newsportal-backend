using FluentValidation;
using NewsPortal.Application.Common;
using NewsPortal.Application.DTOs.News;

namespace NewsPortal.Application.Validators.News;

public sealed class CreateNewsDtoValidator
    : AbstractValidator<CreateNewsDto>
{
    public CreateNewsDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage(
                "عنوان خبر الزامی است و نباید بیشتر از 200 کاراکتر باشد.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(10000)
            .WithMessage(
                "محتوای خبر الزامی است و نباید بیشتر از 10000 کاراکتر باشد.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage(
                "دسته بندی معتبر نیست.");

        RuleForEach(x => x.ImageFiles)
     .Must(file => file.Length > 0)
     .WithMessage("فایل تصویر خالی است.")

     .Must(file =>
         file.Length <= FileUploadLimits.MaxImageSize)
     .WithMessage(
         FileUploadLimits.MaxImageSizeMessage)

     .Must(file =>
     {
         var extension =
             Path.GetExtension(file.FileName);

         return !string.IsNullOrWhiteSpace(extension)
             && FileUploadLimits
                 .AllowedImageExtensions
                 .Contains(
                     extension,
                     StringComparer.OrdinalIgnoreCase);
     })
     .WithMessage(
         FileUploadLimits.AllowedImageExtensionsMessage);
    }
}