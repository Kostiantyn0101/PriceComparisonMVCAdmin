using PriceComparisonMVCAdmin.Models.DTOs.Request.Seller;

namespace PriceComparisonMVCAdmin.Models.ViewModels.Seller
{
    public class ProductReferenceStatisticsViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SellerId { get; set; }
        public List<ProductSellerReferenceClickResponseModel> Results { get; set; } = new();
    }
}
