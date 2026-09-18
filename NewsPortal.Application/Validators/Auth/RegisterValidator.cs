using FluentValidation;
using NewsPortal.Application.DTOs.Auth;

namespace NewsPortal.Application.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("نام و نام خانوادگی الزامی است.")
                .MaximumLength(100).WithMessage("نام و نام خانوادگی نمی‌تواند بیشتر از 100 کاراکتر باشد.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("نام کاربری الزامی است.")
                .MinimumLength(3).WithMessage("نام کاربری باید حداقل 3 کاراکتر باشد.")
                .MaximumLength(50).WithMessage("نام کاربری نمی‌تواند بیشتر از 50 کاراکتر باشد.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل الزامی است.")
                .EmailAddress().WithMessage("فرمت ایمیل معتبر نیست.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز عبور الزامی است.")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل 8 کاراکتر باشد.")
                .MaximumLength(100).WithMessage("رمز عبور نمی‌تواند بیشتر از 100 کاراکتر باشد.");
        }
    }
}
