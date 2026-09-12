using FluentValidation;
using NewsPortal.Application.DTOs.Newsletter;

namespace NewsPortal.Application.Validators.Newsletter;

public class SubscribeNewsletterValidator
    : AbstractValidator<SubscribeNewsletterDto>
{
    public SubscribeNewsletterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("ایمیل الزامی است.")
            .EmailAddress()
            .WithMessage("فرمت ایمیل نامعتبر است.")
            .MaximumLength(320)
            .WithMessage("ایمیل نمی‌تواند بیشتر از 320 کاراکتر باشد.");
    }
}