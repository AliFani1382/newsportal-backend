using FluentValidation;
using NewsPortal.Application.DTOs.Tags;

namespace NewsPortal.Application.Validators.Tags;

public class UpdateTagValidator : AbstractValidator<UpdateTagDto>
{
    public UpdateTagValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("نام برچسب الزامی است.")
            .MaximumLength(100)
            .WithMessage("نام برچسب نمی‌تواند بیشتر از 100 کاراکتر باشد.");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug الزامی است.")
            .MaximumLength(150)
            .WithMessage("Slug نمی‌تواند بیشتر از 150 کاراکتر باشد.");
    }
}