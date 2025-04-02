using FluentValidation;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Product
{
    public class BaseProductViewModelValidator : AbstractValidator<BaseProductViewModel>
    {
        public BaseProductViewModelValidator()
        {
            RuleFor(x => x.BaseProduct)
                           .SetValidator(new BaseProductFormModelValidator());
        }
    }
}
