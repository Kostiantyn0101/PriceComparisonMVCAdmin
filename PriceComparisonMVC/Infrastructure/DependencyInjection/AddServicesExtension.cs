using PriceComparisonMVC.Models.Configuration;
using PriceComparisonMVC.Services;
using System.Net.Http.Headers;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddServicesExtension
    {
        public static void AddServices(this WebApplicationBuilder builder) {

            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>();

            builder.Services.AddHttpClient<IApiService, ApiService>(client =>
            {
                client.BaseAddress = new Uri(jwtConfig?.Issuer!);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri(jwtConfig?.Issuer!);
            });

            builder.Services.AddSingleton<TokenManager>();
        }
    }
}
