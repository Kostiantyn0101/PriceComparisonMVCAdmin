using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IApiRequestService _apiRequestService;

        public CategoryService(IApiRequestService apiRequestService)
        {
            _apiRequestService = apiRequestService;
        }

        public async Task<List<CategoryResponseModel>> GetFilteredCategoryAsync()
        {
            var categories = await _apiRequestService.GetAllCategoriesAsync();
            return categories?.Where(c => c.ParentCategoryId.HasValue).ToList()!;
        }

        public async Task<Dictionary<CategoryResponseModel, List<CategoryResponseModel>>> GetGroupedCategoriesAsync()
        {
            var categories = await _apiRequestService.GetAllCategoriesAsync();

            if (!categories.Any())
            {
                return new Dictionary<CategoryResponseModel, List<CategoryResponseModel>>();
            }

            var parentCategories = categories.Where(c => c.ParentCategoryId == null).ToList();

            var groupedCategories = parentCategories.ToDictionary(
                parent => parent,
                parent => categories.Where(c => c.ParentCategoryId == parent.Id).ToList()
            );

            return groupedCategories;
        }
    }
}
