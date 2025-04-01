using FluentValidation;
using PriceComparisonMVCAdmin.Models.ViewModels.Category;
using PriceComparisonWebAPI.Infrastructure.Validation.Category;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Category
{
    public class CategoryCreateViewModelValidator : AbstractValidator<CategoryCreateViewModel>
    {
        public CategoryCreateViewModelValidator()
        {
            RuleFor(x => x.Category)
                .SetValidator(new CategoryCreateRequestModelValidator());
        }
    }
}
