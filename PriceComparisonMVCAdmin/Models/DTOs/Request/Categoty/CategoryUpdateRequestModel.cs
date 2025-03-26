namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty
{
    public class CategoryUpdateRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IFormFile? NewImage { get; set; }
        public bool DeleteCurrentImage { get; set; }
        public IFormFile? NewIcon { get; set; }
        public bool DeleteCurrentIcon { get; set; }
        public int? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsParent { get; set; }
    }
}
