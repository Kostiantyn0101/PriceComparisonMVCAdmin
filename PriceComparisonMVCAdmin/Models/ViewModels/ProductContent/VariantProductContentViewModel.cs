using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;

namespace PriceComparisonMVCAdmin.Models.ViewModels.ProductContent
{
    public class VariantProductContentViewModel
    {
        public int ProductId { get; set; }
        public int BaseProductId { get; set; }
        public string BaseProductTitle { get; set; } = null!;
        public string ProductVariantTitle { get; set; } = null!;
        public List<ProductImageResponseModel> Images { get; set; }
    }
}
