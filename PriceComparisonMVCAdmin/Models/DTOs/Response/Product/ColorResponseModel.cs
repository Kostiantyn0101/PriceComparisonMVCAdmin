namespace PriceComparisonMVCAdmin.Models.DTOs.Response.Product
{
    public class ColorResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }
        public string? GradientCode { get; set; }
    }
}
