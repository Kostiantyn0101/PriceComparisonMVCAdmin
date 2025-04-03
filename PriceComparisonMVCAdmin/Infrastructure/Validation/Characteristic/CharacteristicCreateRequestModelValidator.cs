using FluentValidation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Characteristic;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Characteristic
{
    public class CharacteristicCreateRequestModelValidator : AbstractValidator<CharacteristicCreateRequestModel>
    {
        public CharacteristicCreateRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Назва характеристики є обов’язковою.")
                .MaximumLength(100).WithMessage("Назва характеристики не повинна перевищувати 100 символів.");

            RuleFor(x => x.DataType)
                .NotEmpty().WithMessage("Тип даних є обов’язковим.");

            RuleFor(x => x.CharacteristicGroupId)
                .GreaterThan(0).WithMessage("Ідентифікатор групи характеристик має бути більшим за 0.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThan(0).WithMessage("Порядок відображення має бути більшим за 0.");
        }
    }
}
