using PriceComparisonMVCAdmin.Models.DTOs.Response;
using System.Text.Json;

namespace PriceComparisonMVCAdmin.Services.Helper
{
    public class ApiResponseDeserializerService : IApiResponseDeserializerService
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiResponseDeserializerService()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public T? DeserializeData<T>(GeneralApiResponseModel response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response), "Response cannot be null.");

            if (response.Data == null)
                throw new Exception("API response does not contain data.");

            try
            {
                // Спочатку перевіряємо, чи `Data` вже є потрібним типом
                if (response.Data is JsonElement jsonElement)
                {
                    // Якщо `Data` є JSON-об'єктом, потрібно його десеріалізувати
                    return JsonSerializer.Deserialize<T>(jsonElement.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                // Якщо `Data` це рядок, пробуємо десеріалізувати з нього
                if (response.Data is string jsonString)
                {
                    return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                // Якщо `Data` вже є об'єктом, намагаємось його конвертувати
                var serializedData = JsonSerializer.Serialize(response.Data);
                return JsonSerializer.Deserialize<T>(serializedData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to deserialize response data: {ex.Message}");
            }
        }

    }

}
