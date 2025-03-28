using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonMVCAdmin.Services
{
    public interface IProductModerationService
    {
        Task<ModerationViewModel> GetModerationViewModelAsync();
    }
}
