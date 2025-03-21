namespace PriceComparisonMVCAdmin.Models.ProductContent
{
    public class ProductImageDeleteViewModel
    {
        public int ProductId { get; set; }
        public List<int> ProductImageIds { get; set; } = new List<int>();
    }
}
