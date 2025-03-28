using AutoMapper;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;
using PriceComparisonMVCAdmin.Services.ApiServices;
using PriceComparisonMVCAdmin.Services.Helper;

namespace PriceComparisonMVCAdmin.Services
{
    public class VariantProductService : IVariantProductService
    {
        private readonly IApiRequestService _apiRequestService;
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;
        private readonly IMapper _mapper;

        public VariantProductService(IApiRequestService apiRequestService, IApiResponseDeserializerService apiResponseDeserializerService, IMapper mapper)
        {
            _apiRequestService = apiRequestService;
            _apiResponseDeserializerService = apiResponseDeserializerService;
            _mapper = mapper;
        }
        public async Task<(ProductResponseModel? product, string? errorMessage)> CreateProductVariantAsync(ProductCreateRequestModel model)
        {
            var response = await _apiRequestService.CreateProductVariantAsync(model);
            var product = _apiResponseDeserializerService.DeserializeData<ProductResponseModel>(response);
            if (response.ReturnCode != AppSuccessCodes.CreateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return (null, response.Message);
            }
            return (product, null);
        }

        public async Task<CreateVariantViewModel> GetCreateVariantViewModelAsync(int baseProductId)
        {
            var groupTypes = await _apiRequestService.GetAllProductGroupTypesAsync();

            var colors = await _apiRequestService.GetAllColorsAsync();

            var baseProduct = await _apiRequestService.GetBaseProductByIdAsync(baseProductId);

            return new CreateVariantViewModel
            {
                ProductVariant = new ProductCreateRequestModel
                {
                    BaseProductId = baseProductId
                },
                GroupTypes = groupTypes,
                Colors = colors,
                BaseProduct = baseProduct
            };
        }

        public async Task<EditVariantViewModel?> GetEditVariantViewModelAsync(int variantId)
        {
            var product = await _apiRequestService.GetProductVariantByIdAsync(variantId);
            if (product == null)
            {
                return null;
            }

            int baseProductId = product.BaseProductId;

            var groupTypes = await _apiRequestService.GetAllProductGroupTypesAsync();
            var colors = await _apiRequestService.GetAllColorsAsync();
            var baseProduct = await _apiRequestService.GetBaseProductByIdAsync(baseProductId);

            var viewModel = new EditVariantViewModel
            {
                ProductVariant = _mapper.Map<ProductUpdateRequestModel>(product),
                GroupTypes = groupTypes,
                Colors = colors,
                BaseProduct = baseProduct,
                SelectedGroupTypeId = product.ProductGroup.ProductGroupTypeId
            };

            return viewModel;
        }

        public string GetVariantTitle(ProductResponseModel product)
        {
            if (product == null) return string.Empty;

            var title = product.ProductGroup?.Name ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(product.ModelNumber))
                title += $" | модель - {product.ModelNumber}";

            if (!string.IsNullOrWhiteSpace(product.GTIN))
                title += $" | GTIN - {product.GTIN}";
            else if (!string.IsNullOrWhiteSpace(product.UPC))
                title += $" | UPC - {product.UPC}";

            return title;
        }

        public async Task<(bool Success, string Message)> ReassignVariantToBaseAsync(int variantId, int newBaseProductId)
        {
            var variant = await _apiRequestService.GetProductVariantByIdAsync(variantId);
            if (variant == null)
                return (false, "Продукт не знайдено.");


            var updateModel = _mapper.Map<ProductUpdateRequestModel>(variant);
            updateModel.BaseProductId = newBaseProductId;

            var response = await _apiRequestService.UpdateProductVariantAsync(updateModel);

            return (response.ReturnCode == AppSuccessCodes.UpdateSuccess, response.Message);
        }
    }
}
