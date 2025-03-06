using PriceComparisonMVC.Models.Response;

namespace PriceComparisonMVC.Models.Categories
{
    public class CategiryListModel
    {
        public CategoryResponseModel ParentCategory { get; set; }

        public List<ProductToCategiriesListModel> ProductToCategiries { get; set; }
    }
}
