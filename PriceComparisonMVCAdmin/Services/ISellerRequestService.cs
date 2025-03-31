using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;

namespace PriceComparisonMVCAdmin.Services
{
    public interface ISellerRequestService
    {
        Task<SellerRequestResponseModel> GetSellerRequestAsync(int id);
        Task<GeneralApiResponseModel> ApproveSellerRequest(SellerRequestProcessRequestModel model);
        Task<List<SellerRequestResponseModel>> GetAllSellerRequests();
        Task<SellerRequestResponseModel?> GetSellerRequestInfoPartialViewModelAsync(int id);
        Task<GeneralApiResponseModel> UpdateSellerRequestAsync(SellerRequestResponseModel model);

    }
}
