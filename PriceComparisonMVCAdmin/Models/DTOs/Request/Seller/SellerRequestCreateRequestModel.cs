namespace PriceComparisonMVCAdmin.Models.Request.Seller
{
    public class SellerRequestCreateRequestModel
    {
        public int UserId { get; set; }
        public string StoreName { get; set; }
        public string WebsiteUrl { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string? StoreComment { get; set; }
    }
}
