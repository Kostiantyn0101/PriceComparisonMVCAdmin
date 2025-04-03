using FluentValidation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Category
{
    public class CategoryUpdateRequestModelValidator : AbstractValidator<CategoryUpdateRequestModel>
    {
        public CategoryUpdateRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Назва категорії є обов’язковою.")
                .MaximumLength(100).WithMessage("Назва категорії не повинна перевищувати 100 символів.");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(1).When(x => x.ParentCategoryId.HasValue)
                .WithMessage("Ідентифікатор батьківської категорії має бути 1 або більше.");

            RuleFor(x => x.NewImage).ImageFileRules();

            RuleFor(x => x.NewIcon).ImageFileRules();

            RuleFor(x => x.DisplayOrder)
                .GreaterThan(0).WithMessage("Порядок відображення має бути більшим за 0.");
        }
    }
}
