namespace PriceComparisonMVCAdmin.Models.ViewModels.Seller
{
    public class SellerAuctionRatesViewModel
    {
        public int SellerId { get; set; }
        public List<CategoryAuctionRateViewModel> Items { get; set; } = new List<CategoryAuctionRateViewModel>();
    }
}
