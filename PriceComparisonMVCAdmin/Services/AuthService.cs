using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PriceComparisonMVCAdmin.Models.Response;
using PriceComparisonMVCAdmin.Models.Request;

namespace PriceComparisonMVCAdmin.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApiService _apiService;
        private readonly TokenManager _tokenManager;

        public AuthService(IApiService apiService, TokenManager tokenManager)
        {
            _apiService = apiService;
            _tokenManager = tokenManager;
        }

        public async Task<bool> LoginAsync(LoginResponseModel login)
        {
            try
            {
                var response = await _apiService.PostAsync<LoginResponseModel, JsonElement>("api/Auth/login", login);

                var token = response.GetProperty("token").GetString();
                var refreshToken = response.GetProperty("refreshToken").GetString();

                _tokenManager.SetToken(token, refreshToken, login.RememberMe);

                return true;
            }
            catch (Exception)
            {
                return false; 
            }
        }


        public async Task LogoutAsync()
        {
            _tokenManager.ClearToken();
        }


        public async Task<string?> RegisterAsync(RegisterModel model)
        {
            try
            {
                Console.WriteLine($"📤 Відправляємо JSON: {JsonSerializer.Serialize(model)}");

                var response = await _apiService.PostAsync<RegisterModel, JsonElement>("api/Auth/register", model);
                return null; // Реєстрація успішна
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Помилка реєстрації: {ex.Message}");
                return "Не вдалося зареєструватися. Можливо, email вже використовується або дані некоректні.";
            }
        }




        //public string GetAuthToken()
        //{
        //    return _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];
        //}
    }
}
