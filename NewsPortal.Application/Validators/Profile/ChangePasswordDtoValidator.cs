using FluentValidation;
using NewsPortal.Application.DTOs.Profile;

namespace NewsPortal.Application.Validators.Profile;

public sealed class ChangePasswordDtoValidator
    : AbstractValidator<ChangedPasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("رمز عبور فعلی الزامی است.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage(".رمز عبور جدید الزامی است")
            .MinimumLength(8)
             .WithMessage(".رمز عبور جدید باید حداقل 8 کاراکتر باشد");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage(".تکرار رمز عبور الزامی است")
           .Equal(x => x.NewPassword)
             .WithMessage(".رمز عبور جدید و تکرار آن یکسان نیست");

    }
}
