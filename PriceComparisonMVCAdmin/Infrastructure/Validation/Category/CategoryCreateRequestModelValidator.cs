using FluentValidation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Category
{
    public class CategoryCreateRequestModelValidator : AbstractValidator<CategoryCreateRequestModel>
    {
        public CategoryCreateRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Назва категорії є обов’язковою.")
                .MaximumLength(100).WithMessage("Довжина назви категорії має бути меншою за 100 символів.");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThanOrEqualTo(1).When(x => x.ParentCategoryId.HasValue)
                .WithMessage("Ідентифікатор батьківської категорії має бути 1 або більшим.");

            RuleFor(x => x.Image).ImageFileRules();

            RuleFor(x => x.Icon).ImageFileRules();

            RuleFor(x => x.DisplayOrder)
                .GreaterThan(0).WithMessage("Порядок відображення має бути більшим за 0.");

        }
    }

}
