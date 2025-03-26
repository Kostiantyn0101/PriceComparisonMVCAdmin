namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Auction
{
    public class AuctionClickRateRequestModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
        public decimal ClickRate { get; set; }
    }
}
