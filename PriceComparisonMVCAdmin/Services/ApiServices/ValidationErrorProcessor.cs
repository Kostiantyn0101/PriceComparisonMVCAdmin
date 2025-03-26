using Microsoft.AspNetCore.Mvc.ModelBinding;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Services.ApiServices;
using System.Text.Json;

public class ValidationErrorProcessor : IValidationErrorProcessor
{
    public bool TryProcessErrors(GeneralApiResponseModel response, ModelStateDictionary modelState)
    {
        if (response.ReturnCode != "ValidationError")
            return false;

        Dictionary<string, string[]>? errors = null;

        if (response.Data is JsonElement jsonElement)
        {
            var validationModel = jsonElement.Deserialize<ApiValidationErrorResponseModel>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            errors = validationModel?.Errors;
        }
        else if (response.Data is ApiValidationErrorResponseModel validationModel)
        {
            errors = validationModel.Errors;
        }

        if (errors != null)
        {
            foreach (var (key, messages) in errors)
            {
                foreach (var msg in messages)
                {
                    modelState.AddModelError(key, msg);
                }
            }

            modelState.AddModelError(string.Empty, response.Message);
            return true;
        }

        return false;
    }
}
