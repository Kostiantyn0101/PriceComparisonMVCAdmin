using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PriceComparisonMVC.Models;

namespace PriceComparisonMVC.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public AuthService(HttpClient httpClient, TokenManager tokenManager)
        {
            _httpClient = httpClient;
            _tokenManager = tokenManager;
        }

        public async Task<bool> LoginAsync(LoginResponseModel login)
        {
            var requestPayload = login;
            var content = new StringContent(JsonSerializer.Serialize(requestPayload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/login", content);
            if (!response.IsSuccessStatusCode) return false;

            var responseData = await JsonSerializer.DeserializeAsync<JsonElement>(await response.Content.ReadAsStreamAsync());

            var token = responseData.GetProperty("token").GetString();
            var refreshToken = responseData.GetProperty("refreshToken").GetString();

            _tokenManager.SetToken(token, refreshToken);
            return true;
        }
    }
}
