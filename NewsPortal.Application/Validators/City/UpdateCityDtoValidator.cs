using FluentValidation;
using NewsPortal.Application.DTOs.City;

namespace NewsPortal.Application.Validators.City;

public class UpdateCityDtoValidator : AbstractValidator<UpdateCityDto>
{
    public UpdateCityDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("نام شهر نباید خالی باشد.")
            .MaximumLength(50).WithMessage("نام شهر نمی‌تواند بیشتر از ۵۰ کاراکتر باشد.");
    }
}
