using PriceComparisonMVCAdmin.Models.Request;
using PriceComparisonMVCAdmin.Models.Response;
using PriceComparisonMVCAdmin.Services.ApiServices;
using System.Text.Json;

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

        public async Task<string?> LoginAsync(LoginResponseModel login)
        {
            try
            {
                var response = await _apiService.PostAsync<LoginResponseModel, JsonElement>("api/Auth/login", login);

                var token = response.GetProperty("token").GetString();
                var refreshToken = response.GetProperty("refreshToken").GetString();

                _tokenManager.SetToken(token, refreshToken, login.RememberMe);

                return token;
            }
            catch (Exception)
            {
                return null;
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
