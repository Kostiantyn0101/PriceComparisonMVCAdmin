namespace PriceComparisonMVCAdmin.Models.ViewModels.ProductContent
{
    public class ProductImageDeleteViewModel
    {
        public int ProductId { get; set; }
        public List<int> ProductImageIds { get; set; } = new List<int>();
    }
}
