using AutoMapper;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;
using PriceComparisonMVCAdmin.Services.ApiServices;
using PriceComparisonMVCAdmin.Services.Helper;

namespace PriceComparisonMVCAdmin.Services
{
    public class ManagerProductsService : IManagerProductsService
    {
        private readonly IApiRequestService _apiRequestService;
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;
        private readonly IMapper _mapper;
        public ManagerProductsService(IApiRequestService apiRequestService, IMapper mapper,
            IApiResponseDeserializerService apiResponseDeserializerService)
        {
            _apiRequestService = apiRequestService;
            _apiResponseDeserializerService = apiResponseDeserializerService;
            _mapper = mapper;
        }
        public async Task<ModerationViewModel> GetModerationViewModelAsync()
        {
            return new ModerationViewModel
            {
                BaseProducts = await _apiRequestService.GetBaseProductsOnModerationAsync(),
                ProductVariants = await _apiRequestService.GetProductVariantsOnModerationAsync()
            };
        }

        public async Task<List<CategoryResponseModel>> GetFilteredCategoryAsync()
        {
            var categories = await _apiRequestService.GetAllCategoriesAsync();
            var filteredCategories = categories?.Where(c => c.ParentCategoryId.HasValue).ToList();
            return filteredCategories!;
        }

        public async Task<BaseProductViewModel> CreateBaseProductViewModelAsync(BaseProductFormModel baseProduct)
        {
            return new BaseProductViewModel
            {
                Categories = await GetFilteredCategoryAsync(),
                BaseProduct = baseProduct
            };
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
                Categories = await GetFilteredCategoryAsync(),
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

        public async Task<EditCharacteristicsViewModel> GetEditCharacteristicsViewModelAsync(int baseProductId, int? productId)
        {
            BaseProductResponseModel baseProduct;
            ProductResponseModel? product = null;
            if (productId.HasValue)
            {
                product = await _apiRequestService.GetProductVariantByIdAsync(productId.Value);
                baseProduct = await _apiRequestService.GetBaseProductByIdAsync(product.BaseProductId);
            }
            else
            {
                baseProduct = await _apiRequestService.GetBaseProductByIdAsync(baseProductId);
            }
            int categoryId = baseProduct.CategoryId;

            var characteristics = await _apiRequestService.GetCategoryCharacteristicsAsync(categoryId);

            var existingCharacteristics = productId.HasValue
                ? await _apiRequestService.GetCharacteristicsForProductAsync(productId.Value)
                : new();


            var baseProductChars = await _apiRequestService.GetCharacteristicsForBaseProductAsync(baseProductId);

            foreach (var baseChar in baseProductChars)
            {
                if (!existingCharacteristics.Any(ec => ec.CharacteristicId == baseChar.CharacteristicId))
                {
                    existingCharacteristics.Add(baseChar);
                }
            }

            var characteristicViewModels = characteristics.Select(c =>
            {
                var existingCharacteristic = existingCharacteristics.FirstOrDefault(
                    ec => ec.CharacteristicId == c.CharacteristicId);

                return new ProductCharacteristicViewModel
                {
                    Id = existingCharacteristic?.Id ?? 0,
                    BaseProductId = baseProductId,
                    ProductId = productId,
                    CharacteristicId = c.CharacteristicId,
                    ValueText = existingCharacteristic?.ValueText,
                    ValueNumber = existingCharacteristic?.ValueNumber,
                    ValueBoolean = existingCharacteristic?.ValueBoolean ?? false,
                    ValueDate = existingCharacteristic?.ValueDate
                };
            }).ToList();

            string? variantTitle = null;
            if (product != null)
            {
                var model = !string.IsNullOrEmpty(product.ModelNumber) ? $" | модель - {product.ModelNumber}" : "";
                var gtinOrUpc = !string.IsNullOrEmpty(product.GTIN)
                    ? $" | GTIN - {product.GTIN}"
                    : (!string.IsNullOrEmpty(product.UPC) ? $" | UPC - {product.UPC}" : "");

                variantTitle = $"{product.ProductGroup?.Name ?? ""}{model}{gtinOrUpc}";
            }

            return new EditCharacteristicsViewModel
            {
                BaseProductId = baseProduct.Id,
                CategoryId = categoryId,
                ProductId = productId,
                Characteristics = characteristicViewModels,
                CharacteristicsMeta = characteristics,
                BaseProductTitle = baseProduct.Title,
                ProductVariantTitle = variantTitle
            };
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

        public async Task<Dictionary<CategoryResponseModel, List<CategoryResponseModel>>> GetGroupedCategoriesAsync()
        {
            var categories = await _apiRequestService.GetAllCategoriesAsync();

            if (!categories.Any())
            {
                return new Dictionary<CategoryResponseModel, List<CategoryResponseModel>>();
            }

            var parentCategories = categories.Where(c => c.ParentCategoryId == null).ToList();

            var groupedCategories = parentCategories.ToDictionary(
                parent => parent,
                parent => categories.Where(c => c.ParentCategoryId == parent.Id).ToList()
            );

            return groupedCategories;
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

    }
}
