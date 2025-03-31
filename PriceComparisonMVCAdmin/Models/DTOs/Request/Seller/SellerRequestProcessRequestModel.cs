namespace PriceComparisonMVCAdmin.Models.Request.Seller
{
    public class SellerRequestProcessRequestModel
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public string? RefusalReason { get; set; }
    }
}
