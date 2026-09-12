using FluentValidation;
using NewsPortal.Application.DTOs.User;
using NewsPortal.Domain.Constants;

namespace NewsPortal.Application.Validators.User
{
    public class ChangeUserRoleDtoValidator : AbstractValidator<ChangeUserRoleDto>
    {
        public ChangeUserRoleDtoValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage(".نقش الزامی است")
                .Must(role => role == RoleNames.User || role == RoleNames.Admin)
                .WithMessage(".نقش باید یکی از مقادیر User یا Admin باشد");
        }
    }
}
