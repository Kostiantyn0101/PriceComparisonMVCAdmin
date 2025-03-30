using PriceComparisonMVCAdmin.Models.DTOs.Request.Seller;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;

namespace PriceComparisonMVCAdmin.Models.ViewModels.Seller
{
    public class AdminSellerInfoPartialViewModel
    {
        public SellerResponseModel Seller { get; set; } = null!;
        public List<ProductSellerReferenceClickResponseModel> ClickStatistics { get; set; } = new();
    }

}
