namespace PriceComparisonMVCAdmin.Models.DTOs.Response
{
    public class ApiValidationErrorResponseModel
    {
        public int Status { get; set; }
        public string Title { get; set; } = string.Empty;
        public Dictionary<string, string[]> Errors { get; set; } = new();
        public string? TraceId { get; set; }
    }

}
