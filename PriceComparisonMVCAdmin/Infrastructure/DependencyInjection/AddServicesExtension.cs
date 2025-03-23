using PriceComparisonMVCAdmin.Models.Configuration;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Services.Helper;
using System.Net.Http.Headers;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddServicesExtension
    {
        public static void AddServices(this WebApplicationBuilder builder) {

            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IApiResponseDeserializerService, ApiResponseDeserializerService>();
            builder.Services.AddScoped<IApiRequestService, ApiRequestService>();
        }
    }
}
