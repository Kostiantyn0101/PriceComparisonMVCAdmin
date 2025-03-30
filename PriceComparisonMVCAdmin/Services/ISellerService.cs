using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;
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

        //admin methods
        Task<List<SellerResponseModel>> GetAllSellersAsync();
        Task<GeneralApiResponseModel> UpdateAdminSellerAsync(AdminSellerEditViewModel model);
        Task<AdminSellerEditViewModel?> GetAdminSellerEditViewModelAsync(int id);
        Task<AdminSellerInfoPartialViewModel> GetAdminSellerInfoPartialViewModelAsync(int id);
    }
}
