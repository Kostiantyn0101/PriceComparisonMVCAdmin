using FluentValidation;
using PriceComparisonMVCAdmin.Models.Request.Seller;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class SellerRequestCreateRequestModelValidator : AbstractValidator<SellerRequestCreateRequestModel>
    {
        public SellerRequestCreateRequestModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId є обов'язковим")
                .GreaterThan(0).WithMessage("UserId має бути більше 0");

            RuleFor(x => x.StoreName)
                .NotEmpty().WithMessage("Назва магазину є обов'язковою")
                .MaximumLength(255).WithMessage("Назва магазину не може перевищувати 255 символів");

            RuleFor(x => x.WebsiteUrl)
                .NotEmpty().WithMessage("URL сайту є обов'язковим")
                .MaximumLength(2083).WithMessage("URL сайту не може перевищувати 2083 символи")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("URL сайту має бути дійсною URL-адресою");

            RuleFor(x => x.ContactPerson)
                .NotEmpty().WithMessage("Ім’я контактної особи є обов’язковим")
                .MaximumLength(100).WithMessage("Ім’я контактної особи не може перевищувати 100 символів");

            RuleFor(x => x.ContactPhone)
                .NotEmpty().WithMessage("Контактний телефон є обов’язковим")
                .MaximumLength(20).WithMessage("Контактний телефон не може перевищувати 20 символів")
                .Matches(@"^\+?[0-9\s\-\(\)]+$").WithMessage("Контактний телефон має бути дійсним номером");

            RuleFor(x => x.StoreComment)
                .MaximumLength(1000).WithMessage("Коментар до магазину не може перевищувати 1000 символів")
                .When(x => x.StoreComment != null);

        }
    }
}
