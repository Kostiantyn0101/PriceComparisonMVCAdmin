using Microsoft.IdentityModel.Tokens;
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

            builder.Services.AddHttpClient<IApiService, ApiService>();
            builder.Services.AddSingleton<TokenManager>();


            builder.Services.AddHttpClient<IAuthService, AuthService>(async (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(jwtConfig?.Issuer!);

                var tokenManager = serviceProvider.GetRequiredService<TokenManager>();
                var token = await tokenManager.GetTokenAsync(); // Асинхронне отримання токена

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            });


            //builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            //{
            //    client.BaseAddress = new Uri(jwtConfig?.Issuer!);
            //});

            builder.Services.AddSingleton<TokenManager>();
        }
    }
}
