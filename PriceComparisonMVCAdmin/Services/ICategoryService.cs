using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;

namespace PriceComparisonMVCAdmin.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseModel>> GetFilteredCategoryAsync();
        Task<Dictionary<CategoryResponseModel, List<CategoryResponseModel>>> GetGroupedCategoriesAsync();
        Task<(List<CategoryResponseModel> Parents, List<CategoryResponseModel> Children)> GetParentAndChildCategoriesAsync();
        Task<List<CategoryResponseModel>> GetAllAsync();
    }
}
