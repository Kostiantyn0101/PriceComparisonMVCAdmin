namespace PriceComparisonMVCAdmin.Models.ViewModels.Seller
{
    public class AdminSellerEditViewModel
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsActive { get; set; }
        public string? CurrentLogoImageUrl { get; set; }
        public decimal AccountBalance { get; set; }
        public IFormFile? NewLogoImage { get; set; }
        public string? ApiKey { get; set; }
        public bool PublishPriceList { get; set; }
    }
}
