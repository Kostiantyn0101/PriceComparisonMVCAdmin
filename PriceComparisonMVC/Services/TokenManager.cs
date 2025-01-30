using System.Text.Json;
using System.Text;

namespace PriceComparisonMVC.Services
{
    public class TokenManager
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenManager(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetAccessToken() => _httpContextAccessor.HttpContext?.Request.Cookies["token"];
        public void SetToken(string accessToken, string refreshToken)
        {
            var context = _httpContextAccessor.HttpContext;

            context?.Response.Cookies.Append("token", accessToken, new CookieOptions { HttpOnly = true });
            context?.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions { HttpOnly = true });
        }

        public async Task<string?> RefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return null;

            var requestData = new { RefreshToken = refreshToken };
            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/refresh-token", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);
                SetToken(tokenResponse!.Token, tokenResponse.RefreshToken);
                return tokenResponse.Token;
            }

            return null;
        }
        private class TokenResponse
        {
            public string Token { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
        }
    }
}
