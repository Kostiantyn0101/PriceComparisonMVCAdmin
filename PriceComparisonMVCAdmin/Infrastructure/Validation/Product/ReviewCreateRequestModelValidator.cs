using FluentValidation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Product
{
    public class ReviewCreateRequestModelValidator : AbstractValidator<ReviewCreateRequestModel>
    {
        public ReviewCreateRequestModelValidator()
        {
            RuleFor(x => x.BaseProductId)
                .GreaterThan(0).WithMessage("Базовий продукт має бути обраний");

            RuleFor(x => x.ReviewUrl)
                .NotEmpty().WithMessage("Посилання на огляд є обов'язковим")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Посилання на огляд має бути дійсним URL");
        }
    }
}
