using FluentValidation;
using NewsPortal.Application.DTOs.Comments;

namespace NewsPortal.Application.Validators.Comments;

public sealed class CreateCommentDtoValidator
    : AbstractValidator<CreateCommentDto>
{
    public CreateCommentDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("متن نظر الزامی است.")
            .MaximumLength(1000)
            .WithMessage("متن نظر نمی‌تواند بیشتر از 1000 کاراکتر باشد.");
    }
}