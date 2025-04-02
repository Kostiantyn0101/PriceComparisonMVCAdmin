using FluentValidation;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class BaseProductFormModelValidator : AbstractValidator<BaseProductFormModel>
    {
        public BaseProductFormModelValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Бренд є обов'язковим.")
                .MaximumLength(100).WithMessage("Бренд не може містити більше 100 символів.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Назва є обов'язковою.")
                .MaximumLength(200).WithMessage("Назва не може містити більше 200 символів.");

            RuleFor(x => x.Description)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Опис не може містити більше 500 символів.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Категорія повинна бути обраною.");
        }
    }
}
