using FluentValidation;
using FluentValidation.Validators;
using NewsPortal.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Validators.Category
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام دسته بندی نباید خالی باشد.")
                .MaximumLength(100).WithMessage("نام دسته بندی نباید بیشتر از 100 کاراکتر باشد");
        }
    }
}
