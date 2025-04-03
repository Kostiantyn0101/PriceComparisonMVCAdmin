using FluentValidation;
using PriceComparisonMVCAdmin.Models.ViewModels.Characteristic;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Characteristic
{
    public class CharacteristicUpdateViewModelValidator : AbstractValidator<CharacteristicUpdateViewModel>
    {
        public CharacteristicUpdateViewModelValidator()
        {
            RuleFor(x => x.Characteristic)
                .SetValidator(new CharacteristicUpdateRequestModelValidator());
        }
    }
}
