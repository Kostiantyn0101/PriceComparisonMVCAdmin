using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;

namespace PriceComparisonMVCAdmin.Models.ViewModels.Category
{
    public class CategoryEditViewModel
    {
        public CategoryUpdateRequestModel Category { get; set; } = new();
        public List<CategoryResponseModel> Parents { get; set; } = new();
        public bool IsEditMode => Category.Id > 0;
    }
}
