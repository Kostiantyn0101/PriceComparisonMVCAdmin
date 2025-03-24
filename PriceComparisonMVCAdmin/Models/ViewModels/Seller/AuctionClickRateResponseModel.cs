namespace PriceComparisonMVCAdmin.Models.ViewModels.Seller
{
    public class AuctionClickRateResponseModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
        public decimal ClickRate { get; set; }
    }
}
