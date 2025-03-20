using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonMVCAdmin.Models.DTOs.Response
{
    public class GeneralApiResponseModel
    {
        public string ReturnCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public static JsonResult GetJsonResult(string returnCode, int httpStatusCode, string? message = null, object? data = null)
        {
            return new JsonResult(new GeneralApiResponseModel
            {
                ReturnCode = returnCode,
                Message = message ?? string.Empty,
                Data = data
            })
            {
                StatusCode = httpStatusCode
            };
        }
    }
}
