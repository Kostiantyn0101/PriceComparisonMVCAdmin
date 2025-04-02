using PriceComparisonMVCAdmin.Models.DTOs.Request.Auction;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Characteristic;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Seller;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Characteristics;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using System.Text.Json;

namespace PriceComparisonMVCAdmin.Services.ApiServices
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
        public Task<BaseProductResponseModel> GetBaseProductByIdAsync(int id)
            => GetSafeAsync<BaseProductResponseModel>($"api/BaseProducts/{id}");
        public Task<List<BaseProductResponseModel>> GetBaseProductsOnModerationAsync()
            => GetSafeAsync<List<BaseProductResponseModel>>("api/BaseProducts/onmoderation");
        public Task<List<BaseProductResponseModel>> GetBaseProductByCategoryIdAsync(int id)
            => GetSafeAsync<List<BaseProductResponseModel>>($"api/BaseProducts/bycategory/{id}");
        public Task<GeneralApiResponseModel> CreateBaseProductAsync(BaseProductCreateRequestModel model)
            => PostSafeAsync<BaseProductCreateRequestModel, GeneralApiResponseModel>("api/BaseProducts/create", model);
        public Task<GeneralApiResponseModel> UpdateBaseProductAsync(BaseProductUpdateRequestModel model)
            => SafeRequestAsync<BaseProductUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/BaseProducts/update", model);
        public Task<GeneralApiResponseModel> DeleteBaseProductAsync(int id)
            => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/BaseProducts/delete/{id}", null);


        //Product Variants
        public Task<ProductResponseModel> GetProductVariantByIdAsync(int id)
            => GetSafeAsync<ProductResponseModel>($"api/Products/{id}");
        public Task<List<ProductResponseModel>> GetProductVariantsOnModerationAsync()
            => GetSafeAsync<List<ProductResponseModel>>("api/Products/onmoderation");
        public Task<List<ProductResponseModel>> GetVariantsByBaseProductIdAsync(int baseProductId)
            => GetSafeAsync<List<ProductResponseModel>>($"api/Products/bybaseproduct/{baseProductId}");
        public Task<GeneralApiResponseModel> CreateProductVariantAsync(ProductCreateRequestModel model)
            => PostSafeAsync<ProductCreateRequestModel, GeneralApiResponseModel>("api/Products/create", model);
        public Task<GeneralApiResponseModel> UpdateProductVariantAsync(ProductUpdateRequestModel model)
            => SafeRequestAsync<ProductUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/Products/update", model);
        public Task<GeneralApiResponseModel> DeleteProductVariantAsync(int id)
            => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/Products/delete/{id}", null);


        //Characteristics
        public Task<List<CharacteristicResponseModel>> GetAllCharacteristicsAsync()
            => GetSafeAsync<List<CharacteristicResponseModel>>($"api/Characteristics/getall");
        public Task<CharacteristicResponseModel> GetCharacteristicByIdAsync(int id)
            => GetSafeAsync<CharacteristicResponseModel>($"/api/Characteristics/{id}");
        public Task<List<string>> GetCharacteristicDataTypesAsync()
            => GetSafeAsync<List<string>>($"/api/Characteristics/datatypes");
        public Task<GeneralApiResponseModel> CreateCharacteristicAsync(CharacteristicCreateRequestModel model)
             => PostSafeAsync<CharacteristicCreateRequestModel, GeneralApiResponseModel>("/api/Characteristics/create", model);
        public Task<GeneralApiResponseModel> UpdateCharacteristicAsync(CharacteristicUpdateRequestModel model)
            => SafeRequestAsync<CharacteristicUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "/api/Characteristics/update", model);
        public Task<GeneralApiResponseModel> DeleteCharacteristicAsync(int id)
             => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"/api/Characteristics/delete/{id}", null);
        
        public Task<List<CharacteristicGroupResponseModel>> GetAllCharacteristicGroupsAsync()
            => GetSafeAsync<List<CharacteristicGroupResponseModel>>($"/api/CharacteristicGroups/getall");
        
        public Task<List<CategoryCharacteristicResponseModel>> GetCategoryCharacteristicsAsync(int categoryId)
            => GetSafeAsync<List<CategoryCharacteristicResponseModel>>($"api/CategoryCharacteristics/{categoryId}");
        public Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForProductAsync(int productId)
            => GetSafeAsync<List<ProductCharacteristicUpdateRequestModel>>($"api/ProductCharacteristics/{productId}");
        public Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForBaseProductAsync(int baseProductId)
            => GetSafeAsync<List<ProductCharacteristicUpdateRequestModel>>($"api/ProductCharacteristics/baseproduct/{baseProductId}");
        public Task<GeneralApiResponseModel> CreateProductCharacteristicAsync(ProductCharacteristicCreateRequestModel model)
            => PostSafeAsync<ProductCharacteristicCreateRequestModel, GeneralApiResponseModel>("api/ProductCharacteristics/create", model);
        public Task<GeneralApiResponseModel> UpdateProductCharacteristicAsync(ProductCharacteristicUpdateRequestModel model)
            => SafeRequestAsync<ProductCharacteristicUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/ProductCharacteristics/update", model);
        public Task<GeneralApiResponseModel> DeleteProductCharacteristicAsync(int id)
            => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/ProductCharacteristics/delete/{id}", null);


        //Seller
        public Task<SellerResponseModel> GetSellerByUserIdAsync(int userId)
            => GetSafeAsync<SellerResponseModel>($"api/Seller/getByUserId/{userId}");
        public Task<List<SellerResponseModel>> GetAllSellersAsync()
            => GetSafeAsync<List<SellerResponseModel>>($"api/Seller/getall");
        public Task<SellerResponseModel> GetSellerByIdAsync(int id)
            => GetSafeAsync<SellerResponseModel>($"api/Seller/{id}");
        public Task<GeneralApiResponseModel> UpdateSellerAsync(SellerUpdateRequestModel model)
           => SafeRequestAsync<SellerUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/Seller/update", model, useMultipartFormData: true);


        //SellerProductDetails
        public Task<GeneralApiResponseModel> UploadPriceListAsync(SellerProductXmlRequestModel model)
            => SafeRequestAsync<SellerProductXmlRequestModel, GeneralApiResponseModel>(
         HttpMethod.Post, "api/SellerProductDetails/upload-file", model, useMultipartFormData: true);


        //ProductReferenceClick
        public Task<List<ProductSellerReferenceClickResponseModel>> GetProductReferenceClickAsync(ProductSellerReferenceClickStaisticRequestModel model)
            => PostSafeAsync<ProductSellerReferenceClickStaisticRequestModel, List<ProductSellerReferenceClickResponseModel>>($"api/ProductReferenceClick/statistic", model);


        //AuctionClickRate
        public Task<List<AuctionClickRateResponseModel>> GetAuctionClickRateAsync(int id)
            => GetSafeAsync<List<AuctionClickRateResponseModel>>($"api/AuctionClickRate/getBySellerId/{id}");
        public Task<GeneralApiResponseModel> CreateAuctionClickRateAsync(AuctionClickRateRequestModel model)
            => PostSafeAsync<AuctionClickRateRequestModel, GeneralApiResponseModel>("api/AuctionClickRate/create", model);
        public Task<GeneralApiResponseModel> UpdateAuctionClickRateAsync(AuctionClickRateRequestModel model)
            => SafeRequestAsync<AuctionClickRateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/AuctionClickRate/update", model);


        //Other
        public Task<List<ColorResponseModel>> GetAllColorsAsync()
            => GetSafeAsync<List<ColorResponseModel>>("api/ProductColor/getall");

        //GroupTypes
        public Task<List<ProductGroupTypeResponseModel>> GetAllProductGroupTypesAsync()
            => GetSafeAsync<List<ProductGroupTypeResponseModel>>("api/ProductGroupType/getall");
        public Task<List<ProductGroupTypeResponseModel>> GetGroupsByTypeIdAsync(int id)
            => GetSafeAsync<List<ProductGroupTypeResponseModel>>($"api/ProductGroup/getByProductGroupType/{id}");


        //Categories
        public Task<List<CategoryResponseModel>> GetAllCategoriesAsync()
            => GetSafeAsync<List<CategoryResponseModel>>("api/Categories/getall");
        public Task<CategoryResponseModel> GetCategoryByIdAsync(int id)
            => GetSafeAsync<CategoryResponseModel>($"api/Categories/{id}");
        public Task<GeneralApiResponseModel> CreateCategoryAsync(CategoryCreateRequestModel model)
            => PostSafeAsync<CategoryCreateRequestModel, GeneralApiResponseModel>("api/Categories/create", model);
        public Task<GeneralApiResponseModel> UpdateCategoryAsync(CategoryUpdateRequestModel model)
            => SafeRequestAsync<CategoryUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, "api/Categories/update", model, useMultipartFormData: true);
        public Task<GeneralApiResponseModel> DeleteCategoryAsync(int id)
            => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/Categories/delete/{id}", null);


        //Videos
        public Task<List<BaseProductVideoResponseModel>> GetBaseProductVideosAsync(int baseProductId)
           => GetSafeAsync<List<BaseProductVideoResponseModel>>($"api/ProductVideo/{baseProductId}");
        public Task<GeneralApiResponseModel> CreateBaseProductVideoAsync(ProductVideoCreateRequestModel model)
            => PostSafeAsync<ProductVideoCreateRequestModel, GeneralApiResponseModel>("api/ProductVideo/create", model);
        public Task<GeneralApiResponseModel> DeleteBaseProductVideoAsync(int id)
           => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/ProductVideo/delete/{id}", null);


        //Instructions
        public Task<List<InstructionResponseModel>> GetIstructionAsync(int id)
            => GetSafeAsync<List<InstructionResponseModel>>($"api/Instruction/{id}");
        public Task<GeneralApiResponseModel> CreateIstructionAsync(InstructionCreateRequestModel model)
            => PostSafeAsync<InstructionCreateRequestModel, GeneralApiResponseModel>("api/Instruction/create", model);
        public Task<GeneralApiResponseModel> DeleteIstructionAsync(int id)
           => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/Instruction/delete/{id}", null);


        //Reviews
        public Task<List<ReviewResponseModel>> GetReviewAsync(int id)
            => GetSafeAsync<List<ReviewResponseModel>>($"api/Reviews/{id}");
        public Task<GeneralApiResponseModel> CreateReviewAsync(ReviewCreateRequestModel model)
            => PostSafeAsync<ReviewCreateRequestModel, GeneralApiResponseModel>("api/Reviews/create", model);
        public Task<GeneralApiResponseModel> DeleteReviewAsync(int id)
           => SafeRequestAsync<object, GeneralApiResponseModel>(HttpMethod.Delete, $"api/Reviews/delete/{id}", null);


        //Images
        public Task<List<ProductImageResponseModel>> GetProductImagesAsync(int id)
            => GetSafeAsync<List<ProductImageResponseModel>>($"api/ProductImage/{id}");
        public Task<GeneralApiResponseModel> AddProductImageAsync(ProductImageCreateRequestModel model)
            => SafeRequestAsync<ProductImageCreateRequestModel, GeneralApiResponseModel>(HttpMethod.Post, $"api/ProductImage/add", model, useMultipartFormData: true);
        public Task<GeneralApiResponseModel> DeleteProductImageAsync(ProductImageDeleteRequestModel model)
           => SafeRequestAsync<ProductImageDeleteRequestModel, GeneralApiResponseModel>(HttpMethod.Delete, $"api/ProductImage/delete/", model);
        public Task<GeneralApiResponseModel> SetPrimaryImageAsync(ProductImageSetPrimaryRequestModel model)
           => SafeRequestAsync<ProductImageSetPrimaryRequestModel, GeneralApiResponseModel>(HttpMethod.Put, $"api/ProductImage/setprimary/", model);


        //SellerRequest
        public Task<List<SellerRequestResponseModel>> GetAllSellerRequestsAsync()
            => SafeRequestAsync<object, List<SellerRequestResponseModel>>(HttpMethod.Get, $"/api/SellerRequest/getAll", null);
        public Task<SellerRequestResponseModel> GetSellerRequestByIdAsync(int id)
            => SafeRequestAsync<object, SellerRequestResponseModel>(HttpMethod.Get, $"/api/SellerRequest/{id}", null);
        public Task<List<SellerRequestResponseModel>> GetPendingSellerRequestsAsync()
            => SafeRequestAsync<object, List<SellerRequestResponseModel>>(HttpMethod.Get, $"/api/SellerRequest/getPending", null);
        public Task<GeneralApiResponseModel> ProcessSellerRequestAsync(SellerRequestProcessRequestModel model)
            => SafeRequestAsync<SellerRequestProcessRequestModel, GeneralApiResponseModel>(HttpMethod.Put, $"api/SellerRequest/process/", model);
        public Task<GeneralApiResponseModel> UpdateSellerRequestAsync(SellerRequestUpdateRequestModel model)
           => SafeRequestAsync<SellerRequestUpdateRequestModel, GeneralApiResponseModel>(HttpMethod.Put, $"api/SellerRequest/update/", model);



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
