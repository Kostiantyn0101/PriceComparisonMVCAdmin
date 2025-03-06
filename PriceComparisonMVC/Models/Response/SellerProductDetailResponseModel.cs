namespace PriceComparisonMVC.Models.Response
{
    public class SellerProductDetailResponseModel
    {
        public string StoreName { get; set; } = string.Empty;
        public string? LogoImageUrl { get; set; }
        public decimal PriceValue { get; set; }
        public string ProductStoreUrl { get; set; } = string.Empty;
        public decimal StoreUrlClickRate { get; set; }
    }

}
