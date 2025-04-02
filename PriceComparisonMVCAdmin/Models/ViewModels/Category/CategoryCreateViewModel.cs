using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;

namespace PriceComparisonMVCAdmin.Models.ViewModels.Category
{
    public class CategoryCreateViewModel
    {
        public CategoryCreateRequestModel Category { get; set; } = new();
        public List<CategoryResponseModel> Parents { get; set; } = new();
    }
}
