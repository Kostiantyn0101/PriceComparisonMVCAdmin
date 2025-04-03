using FluentValidation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Product
{
    public class CreateVariantViewModelValidator : AbstractValidator<CreateVariantViewModel>
    {
        public CreateVariantViewModelValidator()
        {
            RuleFor(x => x.ProductVariant)
                           .SetValidator(new ProductCreateRequestModelValidator());

        }
    }
}
