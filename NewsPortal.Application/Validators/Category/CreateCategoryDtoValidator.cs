using FluentValidation;
using NewsPortal.Application.DTOs.Category;

namespace NewsPortal.Application.Validators.Category;

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام دسته‌بندی نباید خالی باشد.")
            .MaximumLength(100).WithMessage("نام دسته‌بندی نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");
    }
}
