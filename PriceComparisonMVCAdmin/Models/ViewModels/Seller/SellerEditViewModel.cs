namespace PriceComparisonMVCAdmin.Models.ViewModels.Seller
{
    public class SellerEditViewModel
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string WebsiteUrl { get; set; }
        public string? CurrentLogoImageUrl { get; set; }
        public IFormFile? NewLogoImage { get; set; }
        public bool PublishPriceList { get; set; }
    }
}
