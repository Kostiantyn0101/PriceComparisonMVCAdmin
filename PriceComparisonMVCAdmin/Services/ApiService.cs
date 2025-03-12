using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace PriceComparisonMVCAdmin.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenManager _tokenManager;

        public ApiService(HttpClient httpClient, TokenManager tokenManager)
        {
            _httpClient = httpClient;
            _tokenManager = tokenManager;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await ExecuteRequestAsync<T>(() => _httpClient.GetAsync(endpoint));
        }


        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest requestData)
        {
            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            return await ExecuteRequestAsync<TResponse>(() => _httpClient.PostAsync(endpoint, content));
        }


        private async Task<T> ExecuteRequestAsync<T>(Func<Task<HttpResponseMessage>> requestFunc)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await requestFunc();

                
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var newToken = await _tokenManager.RefreshTokenAsync();
                    if (!string.IsNullOrEmpty(newToken))
                    {
                        SetAuthorizationHeader();
                        response = await requestFunc();
                    }
                }

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during API request: {ex.Message}");
            }
        }

        public async Task<TResponse> SendAsync<TRequest, TResponse>(
                HttpMethod method,
                string endpoint,
                TRequest? requestData = default,
                bool useMultipartFormData = false)
        {
            // 1. Формуємо HttpRequestMessage
            var request = new HttpRequestMessage(method, endpoint);

            // 2. Якщо є requestData, дивимось, у якому форматі його відправляти
            if (requestData != null)
            {
                if (useMultipartFormData)
                {
                    // Використовуємо multipart/form-data
                    request.Content = BuildMultipartContent(requestData);
                }
                else
                {
                    // Використовуємо application/json
                    var json = JsonSerializer.Serialize(requestData);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
            }

            // 3. Відправляємо запит через допоміжний метод
            return await SendHttpRequestAsync<TResponse>(request);
        }

        private static MultipartFormDataContent BuildMultipartContent<TRequest>(TRequest data)
        {
            var formData = new MultipartFormDataContent();

            // Reflection: ідемо по всіх властивостях TRequest
            var properties = typeof(TRequest).GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(data);
                if (value == null)
                    continue;

                // Якщо властивість — IFormFile
                if (value is IFormFile file)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    // Назва поля — ім’я властивості (prop.Name)
                    formData.Add(fileContent, prop.Name, file.FileName);
                }
                // Якщо це не файл — додаємо як текст
                else
                {
                    formData.Add(new StringContent(value.ToString()!), prop.Name);
                }
            }

            return formData;
        }

        private async Task<TResponse> SendHttpRequestAsync<TResponse>(HttpRequestMessage request)
        {
            try
            {
                // Додаємо заголовок авторизації (Bearer), якщо є токен
                SetAuthorizationHeader();

                // Виконуємо запит
                var response = await _httpClient.SendAsync(request);

                // Перевірка на 401 Unauthorized + рефреш токена
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var newToken = await _tokenManager.RefreshTokenAsync();
                    if (!string.IsNullOrEmpty(newToken))
                    {
                        SetAuthorizationHeader();
                        response = await _httpClient.SendAsync(request);
                    }
                }

                // Кидає виняток, якщо код статусу не 2xx
                response.EnsureSuccessStatusCode();

                // Читаємо відповідь як JSON
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during API request: {ex.Message}");
            }
        }


        private void SetAuthorizationHeader()
        {
            var token = _tokenManager.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}