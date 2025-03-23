using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response;

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

        public Task<List<BaseProductResponseModel>> GetBaseProductsOnModerationAsync()
            => GetSafeAsync<List<BaseProductResponseModel>>("api/BaseProducts/onmoderation");

        public Task<List<ProductResponseModel>> GetProductVariantsOnModerationAsync()
            => _apiService.GetAsync<List<ProductResponseModel>>("api/Products/onmoderation");

        public Task<List<CategoryResponseModel>> GetAllCategoriesAsync()
            => _apiService.GetAsync<List<CategoryResponseModel>>("api/Categories/getall");

        public Task<GeneralApiResponseModel> CreateBaseProductAsync(BaseProductCreateRequestModel model)
            => _apiService.PostAsync<BaseProductCreateRequestModel, GeneralApiResponseModel>("api/BaseProducts/create", model);

        public Task<BaseProductResponseModel> GetBaseProductByIdAsync(int id)
            => GetSafeAsync<BaseProductResponseModel>($"api/BaseProducts/{id}");

        public Task<List<ProductResponseModel>> GetVariantsByBaseProductIdAsync(int baseProductId)
            => _apiService.GetAsync<List<ProductResponseModel>>($"api/Products/bybaseproduct/{baseProductId}");

        public Task<List<ColorResponseModel>> GetAllColorsAsync()
            => _apiService.GetAsync<List<ColorResponseModel>>("api/ProductColor/getall");

        public Task<GeneralApiResponseModel> UpdateBaseProductAsync(BaseProductUpdateRequestModel model)
            => _apiService.SendAsync<BaseProductUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/BaseProducts/update", model);

        public Task<List<CategoryCharacteristicResponseModel>> GetCategoryCharacteristicsAsync(int categoryId)
            => _apiService.GetAsync<List<CategoryCharacteristicResponseModel>>($"api/CategoryCharacteristics/{categoryId}");

        public Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForProductAsync(int productId)
            => _apiService.GetAsync<List<ProductCharacteristicUpdateRequestModel>>($"api/ProductCharacteristics/{productId}");

        public Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForBaseProductAsync(int baseProductId)
            => _apiService.GetAsync<List<ProductCharacteristicUpdateRequestModel>>($"api/ProductCharacteristics/baseproduct/{baseProductId}");

        public Task<GeneralApiResponseModel> CreateCharacteristicAsync(ProductCharacteristicCreateRequestModel model)
            => _apiService.PostAsync<ProductCharacteristicCreateRequestModel, GeneralApiResponseModel>("api/ProductCharacteristics/create", model);

        public Task<GeneralApiResponseModel> UpdateCharacteristicAsync(ProductCharacteristicUpdateRequestModel model)
            => _apiService.SendAsync<ProductCharacteristicUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/ProductCharacteristics/update", model);

        public Task<GeneralApiResponseModel> DeleteCharacteristicAsync(int id)
            => _apiService.DeleteAsync<object, GeneralApiResponseModel>($"api/ProductCharacteristics/delete/{id}");

        public Task<List<ProductGroupTypeResponseModel>> GetAllProductGroupTypesAsync()
            => _apiService.GetAsync<List<ProductGroupTypeResponseModel>>("api/ProductGroupType/getall");

        public Task<GeneralApiResponseModel> CreateProductVariantAsync(ProductCreateRequestModel model)
            => _apiService.PostAsync<ProductCreateRequestModel, GeneralApiResponseModel>("api/Products/create", model);

        public Task<ProductResponseModel?> GetProductVariantByIdAsync(int id)
            => _apiService.GetAsync<ProductResponseModel>($"api/Products/{id}");

        public Task<GeneralApiResponseModel> UpdateProductVariantAsync(ProductUpdateRequestModel model)
            => _apiService.SendAsync<ProductUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/Products/update", model);

        public Task<GeneralApiResponseModel> DeleteProductVariantAsync(int id)
            => _apiService.DeleteAsync<object, GeneralApiResponseModel>($"api/Products/delete/{id}");

        public Task<GeneralApiResponseModel> DeleteBaseProductAsync(int id)
            => _apiService.DeleteAsync<object, GeneralApiResponseModel>($"api/BaseProducts/delete/{id}");


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

        // Safe PUT
        private async Task<TResponse> PutSafeAsync<TRequest, TResponse>(string url, TRequest data) where TResponse : new()
        {
            try
            {
                return await _apiService.SendAsync<TRequest, TResponse>(HttpMethod.Put, url, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"API PUT request failed: {url}");
                return new TResponse();
            }
        }

        // Safe DELETE
        private async Task<TResponse> DeleteSafeAsync<TResponse>(string url) where TResponse : new()
        {
            try
            {
                return await _apiService.DeleteAsync<object, TResponse>(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"API DELETE request failed: {url}");
                return new TResponse();
            }
        }
    }
}
