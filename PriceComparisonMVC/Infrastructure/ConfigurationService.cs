using PriceComparisonMVC.Infrastructure.DependencyInjection;

namespace PriceComparisonMVC.Infrastructure
{
    public class ConfigurationService
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            try
            {
                builder.AddConfiguration();
                builder.AddAuth();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
