using FluentValidation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;

namespace PriceComparisonMVCAdmin.Infrastructure.Validation.Product
{
    public class InstructionCreateRequestModelValidator : AbstractValidator<InstructionCreateRequestModel>
    {
        public InstructionCreateRequestModelValidator()
        {
            RuleFor(x => x.BaseProductId)
                .GreaterThan(0).WithMessage("Базовий продукт має бути обраний");

            RuleFor(x => x.InstructionUrl)
                .NotEmpty().WithMessage("Посилання на інструкцію є обов'язковим.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Посилання на інструкцію має бути дійсною URL-адресою.");
        }
    }
}
