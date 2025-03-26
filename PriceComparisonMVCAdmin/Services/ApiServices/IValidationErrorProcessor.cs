using Microsoft.AspNetCore.Mvc.ModelBinding;
using PriceComparisonMVCAdmin.Models.DTOs.Response;

namespace PriceComparisonMVCAdmin.Services.ApiServices
{
    public interface IValidationErrorProcessor
    {
        bool TryProcessErrors(GeneralApiResponseModel response, ModelStateDictionary modelState);
    }
}
