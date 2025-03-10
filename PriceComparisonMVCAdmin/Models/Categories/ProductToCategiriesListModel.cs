namespace PriceComparisonMVCAdmin.Models.Categories
{
    public class ProductToCategiriesListModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
    }
}
