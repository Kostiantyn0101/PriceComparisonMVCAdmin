using AutoMapper;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Services
{
    public class SellerRequestService : ISellerRequestService
    {
        private readonly IMapper _mapper;
        private readonly IApiRequestService _apiRequestService;

        public SellerRequestService(IApiRequestService apiRequestService,
            IMapper mapper)
        {
            _apiRequestService = apiRequestService;
            _mapper = mapper;
        }

        public async Task<SellerRequestResponseModel> GetSellerRequestAsync(int id)
        {
            return await _apiRequestService.GetSellerRequestByIdAsync(id);
        }

        public async Task<GeneralApiResponseModel> ApproveSellerRequest(SellerRequestProcessRequestModel model)
        {
            return await _apiRequestService.ProcessSellerRequestAsync(model);
        }

        public async Task<List<SellerRequestResponseModel>> GetAllSellerRequests()
        {
            return await _apiRequestService.GetAllSellerRequestsAsync();
        }

        public async Task<SellerRequestResponseModel?> GetSellerRequestInfoPartialViewModelAsync(int id)
        {
            return await _apiRequestService.GetSellerRequestByIdAsync(id);
        }

        public async Task<GeneralApiResponseModel> UpdateSellerRequestAsync(SellerRequestResponseModel model)
        {
            var modelToUpdate = _mapper.Map<SellerRequestUpdateRequestModel>(model);
            var sellerRequest = await _apiRequestService.UpdateSellerRequestAsync(modelToUpdate);
            if (sellerRequest == null)
            {
                return null;
            }
            return sellerRequest;
        }
    }
}
