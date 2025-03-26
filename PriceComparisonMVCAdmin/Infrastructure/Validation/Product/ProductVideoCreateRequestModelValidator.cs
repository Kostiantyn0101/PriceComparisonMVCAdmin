using FluentValidation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Product
{
    public class ProductVideoCreateRequestModelValidator : AbstractValidator<ProductVideoCreateRequestModel>
    {
        public ProductVideoCreateRequestModelValidator()
        {
            RuleFor(x => x.BaseProductId)
                .GreaterThan(0).WithMessage("Базовий продукт має бути обраний");

            RuleFor(x => x.VideoUrl)
                .NotEmpty().WithMessage("Посилання на відео є обов'язковим.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Посилання на відео має бути дійсною URL-адресою.");
        }
    }
}
