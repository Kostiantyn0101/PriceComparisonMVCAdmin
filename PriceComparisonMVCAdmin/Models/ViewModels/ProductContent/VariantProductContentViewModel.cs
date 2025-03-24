using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;

namespace PriceComparisonMVCAdmin.Models.ViewModels.ProductContent
{
    public class VariantProductContentViewModel
    {
        public int ProductId { get; set; }
        public List<ProductImageResponseModel> Images { get; set; }
    }
}
