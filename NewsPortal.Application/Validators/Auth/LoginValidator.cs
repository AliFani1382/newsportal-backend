using FluentValidation;
using NewsPortal.Application.DTOs.Auth;

namespace NewsPortal.Application.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UsernameOrEmail)
                .NotEmpty().WithMessage("نام کاربری یا ایمیل الزامی است.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز عبور الزامی است.");
        }
    }
}
