using PriceComparisonMVCAdmin.Models.Response;

namespace PriceComparisonMVCAdmin.Models.Categories
{
    public class CategiryListModel
    {
        public CategoryResponseModel ParentCategory { get; set; }

        public List<ProductToCategiriesListModel> ProductToCategiries { get; set; }
    }
}
