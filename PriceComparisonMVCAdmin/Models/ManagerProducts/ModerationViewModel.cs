using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;

namespace PriceComparisonMVCAdmin.Models.ManagerProducts
{
    public class ModerationViewModel
    {
        public List<BaseProductResponseModel> BaseProducts { get; set; } = new();
        public List<ProductResponseModel> ProductVariants { get; set; } = new();
    }
}
