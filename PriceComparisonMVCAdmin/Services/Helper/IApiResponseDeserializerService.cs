using PriceComparisonMVCAdmin.Models.DTOs.Response;
using System.Text.Json;

namespace PriceComparisonMVCAdmin.Services.Helper
{
    public interface IApiResponseDeserializerService
    {
        T? DeserializeData<T>(GeneralApiResponseModel response);
    }
}
