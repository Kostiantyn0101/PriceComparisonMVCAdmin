using FluentValidation;
using PriceComparisonMVCAdmin.Models.Request.Seller;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class SellerRequestProcessRequestModelValidator : AbstractValidator<SellerRequestProcessRequestModel>
    {
        public SellerRequestProcessRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id є обов'язковим")
                .GreaterThan(0).WithMessage("Id має бути більше 0");

            RuleFor(x => x.RefusalReason)
                .NotEmpty().When(x => x.IsApproved == false)
                .WithMessage("Причина відмови є обов’язковою, якщо запит не схвалено")
                .MaximumLength(1000).WithMessage("Причина відмови не може перевищувати 1000 символів");

        }
    }
}
