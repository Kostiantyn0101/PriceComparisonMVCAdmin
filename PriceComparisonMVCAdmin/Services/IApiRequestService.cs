using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response;

namespace PriceComparisonMVCAdmin.Services
{
    public interface IApiRequestService
    {
        Task<List<BaseProductResponseModel>> GetBaseProductsOnModerationAsync();
        Task<List<ProductResponseModel>> GetProductVariantsOnModerationAsync();
        Task<List<CategoryResponseModel>> GetAllCategoriesAsync();
        Task<GeneralApiResponseModel> CreateBaseProductAsync(BaseProductCreateRequestModel model);
        Task<BaseProductResponseModel> GetBaseProductByIdAsync(int id);
        Task<List<ProductResponseModel>> GetVariantsByBaseProductIdAsync(int baseProductId);
        Task<List<ColorResponseModel>> GetAllColorsAsync();
        Task<GeneralApiResponseModel> UpdateBaseProductAsync(BaseProductUpdateRequestModel model);
        Task<List<CategoryCharacteristicResponseModel>> GetCategoryCharacteristicsAsync(int categoryId);
        Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForProductAsync(int productId);
        Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForBaseProductAsync(int baseProductId);
        Task<GeneralApiResponseModel> CreateCharacteristicAsync(ProductCharacteristicCreateRequestModel model);
        Task<GeneralApiResponseModel> UpdateCharacteristicAsync(ProductCharacteristicUpdateRequestModel model);
        Task<GeneralApiResponseModel> DeleteCharacteristicAsync(int id);
        Task<List<ProductGroupTypeResponseModel>> GetAllProductGroupTypesAsync();
        Task<GeneralApiResponseModel> CreateProductVariantAsync(ProductCreateRequestModel model);
        Task<ProductResponseModel> GetProductVariantByIdAsync(int id);
        Task<GeneralApiResponseModel> UpdateProductVariantAsync(ProductUpdateRequestModel model);
        Task<GeneralApiResponseModel> DeleteProductVariantAsync(int id);
        Task<GeneralApiResponseModel> DeleteBaseProductAsync(int id);
    }
}
