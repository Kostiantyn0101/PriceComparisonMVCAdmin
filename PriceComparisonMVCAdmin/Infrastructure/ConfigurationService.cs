using PriceComparisonMVCAdmin.Infrastructure.DependencyInjection;
using PriceComparisonWebAPI.Infrastructure.DependencyInjection;

namespace PriceComparisonMVCAdmin.Infrastructure
{
    public class ConfigurationService
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            try
            {
                builder.AddConfiguration();
                builder.AddServices();
                builder.AddAuth();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
