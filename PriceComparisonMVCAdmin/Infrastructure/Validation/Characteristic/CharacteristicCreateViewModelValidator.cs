using FluentValidation;
using PriceComparisonMVCAdmin.Models.ViewModels.Characteristic;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Characteristic
{
    public class CharacteristicCreateViewModelValidator : AbstractValidator<CharacteristicCreateViewModel>
    {
        public CharacteristicCreateViewModelValidator()
        {
            RuleFor(x => x.Characteristic)
                .SetValidator(new CharacteristicCreateRequestModelValidator());
        }
    }
}
