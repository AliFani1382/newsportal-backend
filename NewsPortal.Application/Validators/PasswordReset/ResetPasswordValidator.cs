using FluentValidation;
using NewsPortal.Application.DTOs.PasswordReset;

namespace NewsPortal.Application.Validators.PasswordReset
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("توکن بازیابی الزامی است.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("رمز عبور الزامی است.")
                .MinimumLength(6)
                .WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد.")
                .MaximumLength(100)
                .WithMessage("رمز عبور نمی‌تواند بیشتر از 100 کاراکتر باشد.");
        }
    }
}