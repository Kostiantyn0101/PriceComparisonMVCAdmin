using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Seller;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Auction;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Characteristics;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Characteristic;

namespace PriceComparisonMVCAdmin.Services.ApiServices
{
    public interface IApiRequestService
    {
        //Base Products
        public Task<BaseProductResponseModel> GetBaseProductByIdAsync(int id);
        public Task<List<BaseProductResponseModel>> GetBaseProductsOnModerationAsync();
        public Task<List<BaseProductResponseModel>> GetBaseProductByCategoryIdAsync(int id);
        public Task<GeneralApiResponseModel> CreateBaseProductAsync(BaseProductCreateRequestModel model);
        public Task<GeneralApiResponseModel> UpdateBaseProductAsync(BaseProductUpdateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteBaseProductAsync(int id);


        //Product Variants
        public Task<ProductResponseModel> GetProductVariantByIdAsync(int id);
        public Task<List<ProductResponseModel>> GetProductVariantsOnModerationAsync();
        public Task<List<ProductResponseModel>> GetVariantsByBaseProductIdAsync(int baseProductId);
        public Task<GeneralApiResponseModel> CreateProductVariantAsync(ProductCreateRequestModel model);
        public Task<GeneralApiResponseModel> UpdateProductVariantAsync(ProductUpdateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteProductVariantAsync(int id);


        //Characteristics
        public Task<List<CharacteristicResponseModel>> GetAllCharacteristicsAsync();
        public Task<CharacteristicResponseModel> GetCharacteristicByIdAsync(int id);
        public Task<List<string>> GetCharacteristicDataTypesAsync();
        public Task<GeneralApiResponseModel> CreateCharacteristicAsync(CharacteristicCreateRequestModel model);
        public Task<GeneralApiResponseModel> UpdateCharacteristicAsync(CharacteristicUpdateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteCharacteristicAsync(int id);
        
        public Task<List<CharacteristicGroupResponseModel>> GetAllCharacteristicGroupsAsync();
        
        public Task<List<CategoryCharacteristicResponseModel>> GetCategoryCharacteristicsAsync(int categoryId);
        public Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForProductAsync(int productId);
        public Task<List<ProductCharacteristicUpdateRequestModel>> GetCharacteristicsForBaseProductAsync(int baseProductId);
        public Task<GeneralApiResponseModel> CreateProductCharacteristicAsync(ProductCharacteristicCreateRequestModel model);
        public Task<GeneralApiResponseModel> UpdateProductCharacteristicAsync(ProductCharacteristicUpdateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteProductCharacteristicAsync(int id);


        //Seller
        public Task<SellerResponseModel> GetSellerByUserIdAsync(int userId);
        public Task<List<SellerResponseModel>> GetAllSellersAsync();
        public Task<SellerResponseModel> GetSellerByIdAsync(int id);
        public Task<GeneralApiResponseModel> UpdateSellerAsync(SellerUpdateRequestModel model);


        //SellerProductDetails
        public Task<GeneralApiResponseModel> UploadPriceListAsync(SellerProductXmlRequestModel model);


        //ProductReferenceClick
        public Task<List<ProductSellerReferenceClickResponseModel>> GetProductReferenceClickAsync(ProductSellerReferenceClickStaisticRequestModel model);


        //AuctionClickRate
        public Task<List<AuctionClickRateResponseModel>> GetAuctionClickRateAsync(int id);
        public Task<GeneralApiResponseModel> CreateAuctionClickRateAsync(AuctionClickRateRequestModel model);
        public Task<GeneralApiResponseModel> UpdateAuctionClickRateAsync(AuctionClickRateRequestModel model);


        //Other
        public Task<List<ColorResponseModel>> GetAllColorsAsync();


        //GroupTypes
        public Task<List<ProductGroupTypeResponseModel>> GetAllProductGroupTypesAsync();
        public Task<List<ProductGroupTypeResponseModel>> GetGroupsByTypeIdAsync(int id);


        //Categories
        public Task<List<CategoryResponseModel>> GetAllCategoriesAsync();
        public Task<CategoryResponseModel> GetCategoryByIdAsync(int id);
        public Task<GeneralApiResponseModel> CreateCategoryAsync(CategoryCreateRequestModel model);
        public Task<GeneralApiResponseModel> UpdateCategoryAsync(CategoryUpdateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteCategoryAsync(int id);


        //Videos
        public Task<List<BaseProductVideoResponseModel>> GetBaseProductVideosAsync(int baseProductId);
        public Task<GeneralApiResponseModel> CreateBaseProductVideoAsync(ProductVideoCreateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteBaseProductVideoAsync(int id);


        //Instructions
        public Task<List<InstructionResponseModel>> GetIstructionAsync(int id);
        public Task<GeneralApiResponseModel> CreateIstructionAsync(InstructionCreateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteIstructionAsync(int id);


        //Reviews
        public Task<List<ReviewResponseModel>> GetReviewAsync(int id);
        public Task<GeneralApiResponseModel> CreateReviewAsync(ReviewCreateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteReviewAsync(int id);


        //Images
        public Task<List<ProductImageResponseModel>> GetProductImagesAsync(int id);
        public Task<GeneralApiResponseModel> AddProductImageAsync(ProductImageCreateRequestModel model);
        public Task<GeneralApiResponseModel> DeleteProductImageAsync(ProductImageDeleteRequestModel model);
        public Task<GeneralApiResponseModel> SetPrimaryImageAsync(ProductImageSetPrimaryRequestModel model);


        //SellerRequest
        public Task<List<SellerRequestResponseModel>> GetAllSellerRequestsAsync();
        public Task<SellerRequestResponseModel> GetSellerRequestByIdAsync(int id);
        public Task<List<SellerRequestResponseModel>> GetPendingSellerRequestsAsync();
        public Task<GeneralApiResponseModel> ProcessSellerRequestAsync(SellerRequestProcessRequestModel model);
        public Task<GeneralApiResponseModel> UpdateSellerRequestAsync(SellerRequestUpdateRequestModel model);


    }
}
