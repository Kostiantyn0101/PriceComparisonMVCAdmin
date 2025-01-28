using System.Text.Json;
using System.Text;

namespace PriceComparisonMVC.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAuthorizationHeader(string jwtToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error {response.StatusCode}: {response.ReasonPhrase}");
                }
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch data from endpoint {endpoint}. Error: {ex.Message}");
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error {response.StatusCode}: {response.ReasonPhrase}");
                }
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to post data to endpoint {endpoint}. Error: {ex.Message}");
            }
        }

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error {response.StatusCode}: {response.ReasonPhrase}");
                }
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to put data to endpoint {endpoint}. Error: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete data from endpoint {endpoint}. Error: {ex.Message}");
            }
        }
    }

}
