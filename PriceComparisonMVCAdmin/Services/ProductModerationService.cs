using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Services
{
    public class ProductModerationService : IProductModerationService
    {
        private readonly IApiRequestService _apiRequestService;

        public ProductModerationService(IApiRequestService apiRequestService)
        {
            _apiRequestService = apiRequestService;
        }
        public async Task<ModerationViewModel> GetModerationViewModelAsync()
        {
            return new ModerationViewModel
            {
                BaseProducts = await _apiRequestService.GetBaseProductsOnModerationAsync(),
                ProductVariants = await _apiRequestService.GetProductVariantsOnModerationAsync(),
                SellerRequests = await _apiRequestService.GetPendingSellerRequestsAsync()
            };

        }
    }
}
