namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Product
{
    public class ProductCreateRequestModel
    {
        public string? GTIN { get; set; }
        public string? UPC { get; set; }
        public string? ModelNumber { get; set; }
        public bool IsUnderModeration { get; set; }
        public int BaseProductId { get; set; }
        public int? ColorId { get; set; }
        public bool IsDefault { get; set; }
        public int? ProductGroupId { get; set; }
    }
}
