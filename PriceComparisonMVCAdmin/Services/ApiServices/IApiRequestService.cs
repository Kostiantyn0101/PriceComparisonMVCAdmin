using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.Response.Seller;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Seller;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Auction;

namespace PriceComparisonMVCAdmin.Services.ApiServices
{
    public interface IApiRequestService
    {
        Task<List<BaseProductResponseModel>> GetBaseProductsOnModerationAsync();
        Task<List<ProductResponseModel>> GetProductVariantsOnModerationAsync();

        Task<List<CategoryResponseModel>> GetAllCategoriesAsync();
        Task<CategoryResponseModel> GetCategoryByIdAsync(int id);
        Task<GeneralApiResponseModel> CreateCategoryAsync(CategoryCreateRequestModel model);
        Task<GeneralApiResponseModel> UpdateCategoryAsync(CategoryUpdateRequestModel model);
        Task<GeneralApiResponseModel> DeleteCategoryAsync(int id);

        Task<SellerResponseModel> GetSellerByUserIdAsync(int userId);
        Task<SellerResponseModel> GetSellerByIdAsync(int id);
        Task<GeneralApiResponseModel> UpdateSellerAsync(SellerUpdateRequestModel model);

        Task<GeneralApiResponseModel> UploadPriceListAsync(SellerProductXmlRequestModel model);

        Task<List<AuctionClickRateResponseModel>> GetAuctionClickRateAsync(int id);
        Task<GeneralApiResponseModel> CreateAuctionClickRateAsync(AuctionClickRateRequestModel model);
        Task<GeneralApiResponseModel> UpdateAuctionClickRateAsync(AuctionClickRateRequestModel model);


        Task<List<ProductSellerReferenceClickResponseModel>> GetProductReferenceClickAsync(ProductSellerReferenceClickStaisticRequestModel model);

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

        Task<List<BaseProductVideoResponseModel>> GetBaseProductVideosAsync(int baseProductId);
        Task<List<InstructionResponseModel>> GetIstructionAsync(int id);
        Task<List<ReviewResponseModel>> GetReviewAsync(int id);

        Task<GeneralApiResponseModel> CreateBaseProductVideoAsync(ProductVideoCreateRequestModel model);
        Task<GeneralApiResponseModel> CreateIstructionAsync(InstructionCreateRequestModel model);
        Task<GeneralApiResponseModel> CreateReviewAsync(ReviewCreateRequestModel model);

        Task<GeneralApiResponseModel> DeleteBaseProductVideoAsync(int id);
        Task<GeneralApiResponseModel> DeleteIstructionAsync(int id);
        Task<GeneralApiResponseModel> DeleteReviewAsync(int id);

        Task<List<ProductImageResponseModel>> GetProductImagesAsync(int id);
        Task<GeneralApiResponseModel> AddProductImageAsync(ProductImageCreateRequestModel model);
        Task<GeneralApiResponseModel> DeleteProductImageAsync(ProductImageDeleteRequestModel model);
        Task<GeneralApiResponseModel> SetPrimaryImageAsync(ProductImageSetPrimaryRequestModel model);

    }
}
