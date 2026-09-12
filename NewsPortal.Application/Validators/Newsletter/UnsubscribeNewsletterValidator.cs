using FluentValidation;
using NewsPortal.Application.DTOs.Newsletter;

namespace NewsPortal.Application.Validators.Newsletter;

public class UnsubscribeNewsletterValidator
    : AbstractValidator<UnsubscribeNewsletterDto>
{
    public UnsubscribeNewsletterValidator()
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