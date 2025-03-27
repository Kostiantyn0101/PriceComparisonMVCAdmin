using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonMVCAdmin.Services
{
    public interface IManagerProductsService
    {
        Task<ModerationViewModel> GetModerationViewModelAsync();
        Task<List<CategoryResponseModel>> GetFilteredCategoryAsync();
        Task<BaseProductViewModel?> GetEditBaseProductViewModelAsync(int id);
        Task<BaseProductViewModel> CreateBaseProductViewModelAsync(BaseProductFormModel baseProduct);
        Task<(BaseProductResponseModel? product, string? errorMessage)> CreateBaseProductAsync(BaseProductFormModel formModel);
        Task<(bool success, string? errorMessage)> UpdateBaseProductAsync(BaseProductViewModel model);

        Task<EditCharacteristicsViewModel> GetEditCharacteristicsViewModelAsync(int baseProductId, int? productId);

        Task<CreateVariantViewModel> GetCreateVariantViewModelAsync(int baseProductId);
        Task<EditVariantViewModel?> GetEditVariantViewModelAsync(int variantId);
        Task<(ProductResponseModel? product, string? errorMessage)> CreateProductVariantAsync(ProductCreateRequestModel model);

        Task<Dictionary<CategoryResponseModel, List<CategoryResponseModel>>> GetGroupedCategoriesAsync();
        string GetVariantTitle(ProductResponseModel product);

    }
}
