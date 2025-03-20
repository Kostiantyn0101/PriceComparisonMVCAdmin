using FluentValidation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Product
{
    public class ProductCreateRequestModelValidator : AbstractValidator<ProductCreateRequestModel>
    {
        public ProductCreateRequestModelValidator()
        {
            RuleFor(x => x.GTIN)
                .MaximumLength(15).WithMessage("Довжина GTIN продукту має бути меншою за 15 символів")
                .When(x => x.GTIN != null);

            RuleFor(x => x.UPC)
                .MaximumLength(15).WithMessage("Довжина UPC продукту має бути меншою за 15 символів")
                .When(x => x.UPC != null);

            RuleFor(x => x.ModelNumber)
                .MaximumLength(255).WithMessage("Довжина номера моделі продукту має бути меншою за 255 символів")
                .When(x => x.ModelNumber != null);

            RuleFor(x => x.BaseProductId)
                .GreaterThan(0).WithMessage("Ідентифікатор базового продукту має бути більшим за 0");

            RuleFor(x => x.ColorId)
                .NotNull().WithMessage("Колір є обов’язковим")
                .GreaterThan(0).WithMessage("Ідентифікатор кольору має бути більшим за 0");

            RuleFor(x => x.ProductGroupId)
                .NotNull().WithMessage("Група продуктів є обов’язковою")
                .GreaterThan(0).WithMessage("Ідентифікатор групи продуктів має бути більшим за 0");

        }
    }
}
