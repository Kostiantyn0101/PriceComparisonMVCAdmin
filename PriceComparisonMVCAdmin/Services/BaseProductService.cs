using AutoMapper;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;
using PriceComparisonMVCAdmin.Services.ApiServices;
using PriceComparisonMVCAdmin.Services.Helper;

namespace PriceComparisonMVCAdmin.Services
{
    public class BaseProductService : IBaseProductService
    {
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;
        private readonly IApiRequestService _apiRequestService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public BaseProductService(ICategoryService categoryService, IMapper mapper,
            IApiRequestService apiRequestService,
            IApiResponseDeserializerService apiResponseDeserializerService)
        {
            _apiResponseDeserializerService = apiResponseDeserializerService;
            _apiRequestService = apiRequestService;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public async Task<(BaseProductResponseModel? product, string? errorMessage)> CreateBaseProductAsync(BaseProductFormModel formModel)
        {
            var modelCreate = _mapper.Map<BaseProductCreateRequestModel>(formModel);

            var response = await _apiRequestService.CreateBaseProductAsync(modelCreate);
            var product = _apiResponseDeserializerService.DeserializeData<BaseProductResponseModel>(response);

            if (product == null)
            {
                return (null, "Invalid response data.");
            }
            if (response.ReturnCode != AppSuccessCodes.CreateSuccess &&
                response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return (null, response.Message);
            }
            return (product, null);
        }

        public async Task<BaseProductViewModel> CreateBaseProductViewModelAsync(BaseProductFormModel baseProduct)
        {
            return new BaseProductViewModel
            {
                Categories = await _categoryService.GetFilteredCategoryAsync(),
                BaseProduct = baseProduct
            };
        }

        public async Task<BaseProductViewModel?> GetEditBaseProductViewModelAsync(int id)
        {
            var baseProduct = await _apiRequestService.GetBaseProductByIdAsync(id);
            if (baseProduct == null || baseProduct.Id == 0)
            {
                return null;
            }

            var productVariants = await _apiRequestService.GetVariantsByBaseProductIdAsync(id);
            var productColors = await _apiRequestService.GetAllColorsAsync();

            var formModel = _mapper.Map<BaseProductFormModel>(baseProduct);

            var viewModel = new BaseProductViewModel
            {
                BaseProduct = formModel,
                Categories = await _categoryService.GetFilteredCategoryAsync(),
                productVariants = productVariants,
                productColors = productColors
            };

            return viewModel;
        }

        public async Task<(bool success, string? errorMessage)> UpdateBaseProductAsync(BaseProductViewModel model)
        {
            var modelUpdate = _mapper.Map<BaseProductUpdateRequestModel>(model.BaseProduct);

            var response = await _apiRequestService.UpdateBaseProductAsync(modelUpdate);

            if (response.ReturnCode != AppSuccessCodes.UpdateSuccess &&
                response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return (false, response.Message);
            }

            return (true, null);
        }
    }
}
