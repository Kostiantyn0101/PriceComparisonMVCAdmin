using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonMVCAdmin.Services
{
    public interface IBaseProductService
    {
        Task<BaseProductViewModel?> GetEditBaseProductViewModelAsync(int id);
        Task<BaseProductViewModel> CreateBaseProductViewModelAsync(BaseProductFormModel baseProduct);
        Task<(BaseProductResponseModel? product, string? errorMessage)> CreateBaseProductAsync(BaseProductFormModel formModel);
        Task<(bool success, string? errorMessage)> UpdateBaseProductAsync(BaseProductViewModel model);

    }
}
