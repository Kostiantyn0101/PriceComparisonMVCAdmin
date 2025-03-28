using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonMVCAdmin.Services
{
    public interface IVariantProductService
    {
        Task<CreateVariantViewModel> GetCreateVariantViewModelAsync(int baseProductId);
        Task<EditVariantViewModel?> GetEditVariantViewModelAsync(int variantId);
        Task<(ProductResponseModel? product, string? errorMessage)> CreateProductVariantAsync(ProductCreateRequestModel model);
        Task<(bool Success, string Message)> ReassignVariantToBaseAsync(int variantId, int newBaseProductId);
        string GetVariantTitle(ProductResponseModel product);
    }
}
