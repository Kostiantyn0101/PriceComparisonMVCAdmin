using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using System.Text.Json;

namespace PriceComparisonMVCAdmin.Services
{
    public class ApiRequestService : IApiRequestService
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ApiRequestService> _logger;

        public ApiRequestService(IApiService apiService, ILogger<ApiRequestService> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        //Base Products
        public Task<List<BaseProductResponseModel>> GetBaseProductsOnModerationAsync()
            => GetSafeAsync<List<BaseProductResponseModel>>("api/BaseProducts/onmoderation");
        public Task<BaseProductResponseModel> GetBaseProductByIdAsync(int id)
            => GetSafeAsync<BaseProductResponseModel>($"api/BaseProducts/{id}");
        public Task<GeneralApiResponseModel> CreateBaseProductAsync(BaseProductCreateRequestModel model)
            => PostSafeAsync<BaseProductCreateRequestModel, GeneralApiResponseModel>("api/BaseProducts/create", model);
        public Task<GeneralApiResponseModel> UpdateBaseProductAsync(BaseProductUpdateRequestModel model)
            => SafeRequestAsync<BaseProductUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/BaseProducts/update", model);
        public Task<GeneralApiResponseModel> DeleteBaseProductAsync(int id)
            => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/BaseProducts/delete/{id}", null);


        //Product Variants
        public Task<List<ProductResponseModel>> GetProductVariantsOnModerationAsync()
            => GetSafeAsync<List<ProductResponseModel>>("api/Products/onmoderation");
        public Task<List<ProductResponseModel>> GetVariantsByBaseProductIdAsync(int baseProductId)
            => GetSafeAsync<List<ProductResponseModel>>($"api/Products/bybaseproduct/{baseProductId}");
        public Task<ProductResponseModel> GetProductVariantByIdAsync(int id)
            => GetSafeAsync<ProductResponseModel>($"api/Products/{id}");
        public Task<GeneralApiResponseModel> CreateProductVariantAsync(ProductCreateRequestModel model)
            => PostSafeAsync<ProductCreateRequestModel, GeneralApiResponseModel>("api/Products/create", model);
        public Task<GeneralApiResponseModel> UpdateProductVariantAsync(ProductUpdateRequestModel model)
            => SafeRequestAsync<ProductUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/Products/update", model);
        public Task<GeneralApiResponseModel> DeleteProductVariantAsync(int id)
            => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/Products/delete/{id}", null);


        //Characteristics
        public Task<List<CategoryCharacteristicResponseModel>> GetCategoryCharacteristicsAsync(int categoryId)
            => GetSafeAsync<List<CategoryCharacteristicResponseModel>>($"api/CategoryCharacteristics/{categoryId}");
        public Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForProductAsync(int productId)
            => GetSafeAsync<List<ProductCharacteristicUpdateRequestModel>>($"api/ProductCharacteristics/{productId}");
        public Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForBaseProductAsync(int baseProductId)
            => GetSafeAsync<List<ProductCharacteristicUpdateRequestModel>>($"api/ProductCharacteristics/baseproduct/{baseProductId}");
        public Task<GeneralApiResponseModel> CreateCharacteristicAsync(ProductCharacteristicCreateRequestModel model)
            => PostSafeAsync<ProductCharacteristicCreateRequestModel, GeneralApiResponseModel>("api/ProductCharacteristics/create", model);
        public Task<GeneralApiResponseModel> UpdateCharacteristicAsync(ProductCharacteristicUpdateRequestModel model)
            => SafeRequestAsync<ProductCharacteristicUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/ProductCharacteristics/update", model);
        public Task<GeneralApiResponseModel> DeleteCharacteristicAsync(int id)
            => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/ProductCharacteristics/delete/{id}", null);


        //Other
        public Task<List<ColorResponseModel>> GetAllColorsAsync()
            => GetSafeAsync<List<ColorResponseModel>>("api/ProductColor/getall");
        public Task<List<ProductGroupTypeResponseModel>> GetAllProductGroupTypesAsync()
           => GetSafeAsync<List<ProductGroupTypeResponseModel>>("api/ProductGroupType/getall");
        public Task<List<CategoryResponseModel>> GetAllCategoriesAsync()
            => GetSafeAsync<List<CategoryResponseModel>>("api/Categories/getall");




        // Safe GET
        private async Task<T> GetSafeAsync<T>(string url) where T : new()
        {
            try
            {
                return await _apiService.GetAsync<T>(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"API GET request failed: {url}");
                return new T();
            }
        }

        // Safe POST
        private async Task<TResponse> PostSafeAsync<TRequest, TResponse>(string url, TRequest data) where TResponse : new()
        {
            try
            {
                return await _apiService.PostAsync<TRequest, TResponse>(url, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"API POST request failed: {url}");
                return new TResponse();
            }
        }

        private async Task<TResponse> SafeRequestAsync<TRequest, TResponse>(
            HttpMethod method,
            string url,
            TRequest requestData,
            bool useMultipartFormData = false) where TResponse : new()
        {
            try
            {
                if (method == HttpMethod.Get)
                {
                    return await _apiService.GetAsync<TResponse>(url);
                }
                else if (method == HttpMethod.Delete)
                {
                    return await _apiService.DeleteAsync<TRequest, TResponse>(url, requestData);
                }
                else
                {
                    return await _apiService.SendAsync<TRequest, TResponse>(method, url, requestData, useMultipartFormData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API {Method} request failed: {Url} with data: {Data}",
                    method, url, JsonSerializer.Serialize(requestData));
                return new TResponse();
            }
        }
    }
}
