namespace PriceComparisonMVCAdmin.Models.Response
{
    public class ProductResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Gtin { get; set; } = string.Empty;
        public string? Upc { get; set; }
    }
}