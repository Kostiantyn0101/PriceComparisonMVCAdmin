namespace PriceComparisonMVCAdmin.Models.DTOs.Response.Product
{
    public class ProductResponseModel
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public ProductGroupResponseModel ProductGroup { get; set; }
        public string? ModelNumber { get; set; }
        public string? GTIN { get; set; }
        public string? UPC { get; set; }
        public int? ColorId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsUnderModeration { get; set; }
        public DateTime AddedToDatabase { get; set; }
    }
}
