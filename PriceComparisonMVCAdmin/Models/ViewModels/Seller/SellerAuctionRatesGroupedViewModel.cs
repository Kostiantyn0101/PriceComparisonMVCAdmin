namespace PriceComparisonMVCAdmin.Models.ViewModels.Seller
{
    public class SellerAuctionRatesGroupedViewModel
    {
        public int SellerId { get; set; }

        public Dictionary<ParentCategoryViewModel, List<CategoryAuctionRateViewModel>> GroupedCategories { get; set; }
            = new();
    }
}
