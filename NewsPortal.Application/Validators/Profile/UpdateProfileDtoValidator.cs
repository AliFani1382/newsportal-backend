using FluentValidation;
using NewsPortal.Application.DTOs.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Validators.Profile
{
    public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage(".نام و نام خانوادگی الزامی است")
                .MaximumLength(100)
                .WithMessage(".نام و نام خانوادگی نمیتواند بیشتر از 100 کاراکتر باشد ");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(".ایمیل الزامی است")
                .EmailAddress()
                .WithMessage(".فرمت ایمیل صحیح نیست")
                .MaximumLength(150)
                .WithMessage(".ایمیل نمیتواند بیشتر از 150 کاراکتر باشد");
        }
    }
}
