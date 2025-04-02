using FluentValidation;
using PriceComparisonMVCAdmin.Infrastructure.Validation.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.Category;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;
using PriceComparisonWebAPI.Infrastructure.Validation.Category;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Category
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
