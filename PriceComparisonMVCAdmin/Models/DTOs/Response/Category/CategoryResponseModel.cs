namespace PriceComparisonMVCAdmin.Models.DTOs.Response.Category
{
    public class CategoryResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconUrl { get; set; }
        public int? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsParent { get; set; }
    }
}
