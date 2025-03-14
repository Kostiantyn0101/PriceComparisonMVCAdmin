namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Seller
{
    public class ProductSellerReferenceClickStaisticRequestModel
    {
        public int SellerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
