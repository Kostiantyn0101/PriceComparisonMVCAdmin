using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonMVCAdmin.Models
{
    public class GeneralApiResponseModel
    {
        public string ReturnCode { get; set; }
        public string Message { get; set; }

        public static JsonResult GetJsonResult(string appErrors, int httpStatusCode, string? message = null)
        {
            return new JsonResult(new GeneralApiResponseModel
            {
                ReturnCode = appErrors,
                Message = message ?? string.Empty
            })
            {
                StatusCode = httpStatusCode
            };
        }
    }
}
