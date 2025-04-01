using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;

namespace PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts
{
    public class ModerationViewModel
    {
        public List<BaseProductResponseModel> BaseProducts { get; set; } = new();
        public List<ProductResponseModel> ProductVariants { get; set; } = new();
        public List<SellerRequestResponseModel> SellerRequests { get; set; } = new();
    }
}
