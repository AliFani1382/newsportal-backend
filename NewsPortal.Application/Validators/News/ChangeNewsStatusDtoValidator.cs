using FluentValidation;
using NewsPortal.Application.DTOs.News;

namespace NewsPortal.Application.Validators.News
{
    public class ChangeNewsStatusDtoValidator : AbstractValidator<ChangeNewsStatusDto>
    {
        public ChangeNewsStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(".وضعیت ارسال‌شده معتبر نیست");
        }
    }
}
