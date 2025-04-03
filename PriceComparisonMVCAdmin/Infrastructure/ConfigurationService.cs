using PriceComparisonMVCAdmin.Infrastructure.DependencyInjection;
using PriceComparisonWebAPI.Infrastructure.DependencyInjection;

namespace PriceComparisonMVCAdmin.Infrastructure
{
    public class ConfigurationService
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.AddConfiguration();
            builder.AddFluentValidation();
            builder.AddRazorRuntimeCompilation();
            builder.AddAutoMapper();
            builder.AddServices();
            builder.AddOthers();
            builder.AddAuth();
        }
    }
}
