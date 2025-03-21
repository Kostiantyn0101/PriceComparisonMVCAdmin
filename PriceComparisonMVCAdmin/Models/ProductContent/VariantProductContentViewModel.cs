using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;

namespace PriceComparisonMVCAdmin.Models.ProductContent
{
    public class VariantProductContentViewModel
    {
        public int ProductId { get; set; }
        public List<ProductImageResponseModel> Images { get; set; }
    }
}
