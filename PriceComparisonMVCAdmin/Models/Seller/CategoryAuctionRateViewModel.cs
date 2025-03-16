namespace PriceComparisonMVCAdmin.Models.Seller
{
    public class CategoryAuctionRateViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public int? AuctionClickRateId { get; set; }
        public decimal AuctionClickRate { get; set; }
    }
}
