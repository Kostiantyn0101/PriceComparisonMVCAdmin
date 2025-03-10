namespace PriceComparisonMVCAdmin.Models.Product
{
    public class RelatedProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; } = decimal.Zero;
        public string ImageUrl { get; set; } = string.Empty;
    }

}
