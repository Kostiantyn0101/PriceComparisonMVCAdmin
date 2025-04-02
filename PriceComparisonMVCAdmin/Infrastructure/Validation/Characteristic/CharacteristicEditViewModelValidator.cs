using FluentValidation;
using PriceComparisonMVCAdmin.Models.ViewModels.Characteristic;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Characteristic
{
    public class CharacteristicEditViewModelValidator : AbstractValidator<CharacteristicEditViewModel>
    {
        public CharacteristicEditViewModelValidator()
        {
            RuleFor(x => x.Characteristic)
                .SetValidator(new CharacteristicUpdateRequestModelValidator());
        }
    }
}
