namespace PriceComparisonMVC.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
        public CategoryDto? ParentCategory { get; set; }
        public ICollection<CategoryDto>? Subcategories { get; set; }
    }
}
