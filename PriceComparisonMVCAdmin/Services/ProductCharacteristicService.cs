using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs;
using PriceComparisonMVCAdmin.Services.Helper;
using PriceComparisonMVCAdmin.Services.ApiServices;

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
                BaseProductId = model.BaseProductId,
                ProductId = model.ProductId,
                CharacteristicId = model.CharacteristicId,
                ValueText = !string.IsNullOrEmpty(model.ValueText) ? model.ValueText : null,
                ValueNumber = model.ValueNumber.HasValue ? model.ValueNumber : null,
                ValueDate = model.ValueDate.HasValue ? model.ValueDate : null,
                ValueBoolean = (string.IsNullOrEmpty(model.ValueText) && !model.ValueNumber.HasValue && !model.ValueDate.HasValue)
                    ? model.ValueBoolean : null
            };

            var response = await _apiRequestService.CreateCharacteristicAsync(createModel);
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
                BaseProductId = model.BaseProductId,
                ProductId = model.ProductId,
                CharacteristicId = model.CharacteristicId,
                ValueText = !string.IsNullOrEmpty(model.ValueText) ? model.ValueText : null,
                ValueNumber = model.ValueNumber.HasValue ? model.ValueNumber : null,
                ValueDate = model.ValueDate.HasValue ? model.ValueDate : null,
                ValueBoolean = (string.IsNullOrEmpty(model.ValueText) && !model.ValueNumber.HasValue && !model.ValueDate.HasValue)
                    ? model.ValueBoolean : null
            };

            var response = await _apiRequestService.UpdateCharacteristicAsync(updateModel);
            if (response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                return (false, null, response.Message);
            }

            var updatedCharacteristic = _apiResponseDeserializerService.DeserializeData<ProductCharacteristicResponseModel>(response);
            return (true, updatedCharacteristic?.Id, null);
        }

        public async Task<(bool success, string? errorMessage)> DeleteCharacteristicAsync(int id)
        {
            var response = await _apiRequestService.DeleteCharacteristicAsync(id);
            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess)
            {
                return (false, response.Message);
            }
            return (true, null);
        }
    }
}
