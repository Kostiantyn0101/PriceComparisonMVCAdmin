namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty
{
    public class CategoryCreateRequestModel
    {
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Icon { get; set; }
        public int? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
