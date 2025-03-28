using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Services.ApiServices;
using PriceComparisonMVCAdmin.Services.Helper;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddServicesExtension
    {
        public static void AddServices(this WebApplicationBuilder builder) {

            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IApiResponseDeserializerService, ApiResponseDeserializerService>();
            builder.Services.AddScoped<IProductCharacteristicService, ProductCharacteristicService>();
            builder.Services.AddScoped<IValidationErrorProcessor, ValidationErrorProcessor>();
            builder.Services.AddScoped<IProductModerationService, ProductModerationService>();
            builder.Services.AddScoped<IVariantProductService, VariantProductService>();
            builder.Services.AddScoped<IBaseProductService, BaseProductService>();
            builder.Services.AddScoped<IApiRequestService, ApiRequestService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISellerService, SellerService>();
        }
    }
}
