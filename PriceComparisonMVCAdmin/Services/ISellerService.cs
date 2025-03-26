using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using System.Security.Claims;

namespace PriceComparisonMVCAdmin.Services
{
    public interface ISellerService
    {
        Task<ProductReferenceStatisticsViewModel> GetReferenceStatisticsAsync(DateTime startDate, DateTime endDate, ClaimsPrincipal user);
        Task<SellerEditViewModel?> GetSellerEditViewModelAsync(int id);
        Task<GeneralApiResponseModel?> UpdateSellerAsync(SellerEditViewModel model);
        Task<(bool IsSuccess, string Message)> UploadPriceListAsync(IFormFile? file);
        Task<SellerAuctionRatesGroupedViewModel> GetAuctionRatesViewModelAsync(ClaimsPrincipal user);

    }
}
