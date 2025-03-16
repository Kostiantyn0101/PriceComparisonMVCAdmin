using System.Text.Json;
using System.Text;
using PriceComparisonMVCAdmin.Models.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PriceComparisonMVCAdmin.Services
{
    public class TokenManager
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtConfiguration _options;


        public TokenManager(HttpClient httpClient, 
            IHttpContextAccessor httpContextAccessor, 
            IOptions<JwtConfiguration> options)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;
        }


        public string? GetToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["token"];
        }

        public string? GetUsernameFromToken()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            return usernameClaim?.Value;
        }

        public string? GetUserIdFromToken()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }

        public async Task<string?> GetTokenAsync()
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Cookies["token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                return accessToken;
            }

            return await RefreshTokenAsync();
        }


        public string? GetAccessToken() => _httpContextAccessor.HttpContext?.Request.Cookies["token"];


        public void SetToken(string accessToken, string refreshToken, bool rememberMe)
        {
            var context = _httpContextAccessor.HttpContext;

            double refreshTokenLifetime = rememberMe ? 
                _options.RememberMeRefreshTokenLifetimeHours :
                _options.DefaultRefreshTokenLifetimeHours;

            context?.Response.Cookies.Append("token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeMin)
            });
            context?.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions { 
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(refreshTokenLifetime)
            });
        }

        public void SetToken(string accessToken, string refreshToken)
        {
            SetToken(accessToken, refreshToken, rememberMe: false);
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

        public void ClearToken()
        {
            var context = _httpContextAccessor.HttpContext;
            context?.Response.Cookies.Delete("token");
            context?.Response.Cookies.Delete("refreshToken");
        }
    }
}
