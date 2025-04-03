using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs;
using PriceComparisonMVCAdmin.Services.Helper;
using PriceComparisonMVCAdmin.Services.ApiServices;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonMVCAdmin.Services
{
    public class ProductCharacteristicService : IProductCharacteristicService
    {
        private readonly IApiRequestService _apiRequestService;
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;

        public ProductCharacteristicService(IApiRequestService apiRequestService, IApiResponseDeserializerService apiResponseDeserializerService)
        {
            _apiRequestService = apiRequestService;
            _apiResponseDeserializerService = apiResponseDeserializerService;
        }

        public async Task<(bool success, int? updatedId, string? errorMessage)> CreateCharacteristicAsync(ProductCharacteristicViewModel model)
        {
            var createModel = new ProductCharacteristicCreateRequestModel
            {
                BaseProductId = model.BaseProductId > 0 ? model.BaseProductId : null,
                ProductId = model.ProductId > 0 ? model.ProductId : null,
                CharacteristicId = model.CharacteristicId,
                ValueText = !string.IsNullOrEmpty(model.ValueText) ? model.ValueText : null,
                ValueNumber = model.ValueNumber.HasValue ? model.ValueNumber : null,
                ValueDate = model.ValueDate.HasValue ? model.ValueDate : null,
                ValueBoolean = (string.IsNullOrEmpty(model.ValueText) && !model.ValueNumber.HasValue && !model.ValueDate.HasValue)
                    ? model.ValueBoolean : null
            };

            var response = await _apiRequestService.CreateProductCharacteristicAsync(createModel);
            if (response.ReturnCode != AppSuccessCodes.CreateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return (false, null, response.Message);
            }

            var updatedCharacteristic = _apiResponseDeserializerService.DeserializeData<ProductCharacteristicResponseModel>(response);
            return (true, updatedCharacteristic?.Id, null);
        }

        public async Task<(bool success, int? updatedId, string? errorMessage)> UpdateCharacteristicAsync(ProductCharacteristicViewModel model)
        {
            var updateModel = new ProductCharacteristicUpdateRequestModel
            {
                Id = model.Id,
                BaseProductId = model.BaseProductId > 0 ? model.BaseProductId : null,
                ProductId = model.ProductId > 0 ? model.ProductId : null,
                CharacteristicId = model.CharacteristicId,
                ValueText = !string.IsNullOrEmpty(model.ValueText) ? model.ValueText : null,
                ValueNumber = model.ValueNumber.HasValue ? model.ValueNumber : null,
                ValueDate = model.ValueDate.HasValue ? model.ValueDate : null,
                ValueBoolean = (string.IsNullOrEmpty(model.ValueText) && !model.ValueNumber.HasValue && !model.ValueDate.HasValue)
                    ? model.ValueBoolean : null
            };

            var response = await _apiRequestService.UpdateProductCharacteristicAsync(updateModel);
            if (response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                return (false, null, response.Message);
            }

            var updatedCharacteristic = _apiResponseDeserializerService.DeserializeData<ProductCharacteristicResponseModel>(response);
            return (true, updatedCharacteristic?.Id, null);
        }

        public async Task<(bool success, string? errorMessage)> DeleteCharacteristicAsync(int id)
        {
            var response = await _apiRequestService.DeleteProductCharacteristicAsync(id);
            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess)
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
    }
}
