namespace PriceComparisonMVCAdmin.Models.Response.Seller
{
    public class SellerResponseModel
    {
        public int Id { get; set; }
        public string ApiKey { get; set; }
        public string StoreName { get; set; }
        public string? LogoImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsActive { get; set; }
        public bool PublishPriceList { get; set; }
        public decimal AccoundBalance { get; set; }
        public int UserId { get; set; }
    }
}
