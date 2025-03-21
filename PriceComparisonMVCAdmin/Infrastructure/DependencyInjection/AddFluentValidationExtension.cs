using FluentValidation.AspNetCore;
using FluentValidation;

namespace PriceComparisonMVCAdmin.Infrastructure.DependencyInjection
{
    public static class AddFluentValidationExtension
    {
        public static void AddFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        }
    }
}
