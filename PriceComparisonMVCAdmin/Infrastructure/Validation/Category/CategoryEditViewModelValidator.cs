using FluentValidation;
using PriceComparisonMVCAdmin.Models.ViewModels.Category;
using PriceComparisonWebAPI.Infrastructure.Validation.Category;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Category
{
    public class CategoryEditViewModelValidator : AbstractValidator<CategoryEditViewModel>
    {
        public CategoryEditViewModelValidator()
        {
            RuleFor(x => x.Category)
                .SetValidator(new CategoryUpdateRequestModelValidator());
        }
    }
}
