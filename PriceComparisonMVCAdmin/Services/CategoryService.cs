using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.ViewModels.Category;
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
            var categories = await GetAllAsync();
            return categories?.Where(c => c.ParentCategoryId.HasValue).ToList()!;
        }

        public async Task<Dictionary<CategoryResponseModel, List<CategoryResponseModel>>> GetGroupedCategoriesAsync()
        {
            var categories = await GetAllAsync();

            if (!categories.Any())
            {
                return new Dictionary<CategoryResponseModel, List<CategoryResponseModel>>();
            }

            return categories
            .Where(c => c.ParentCategoryId == null)
            .ToDictionary(
                parent => parent,
                parent => categories.Where(c => c.ParentCategoryId == parent.Id).ToList()
            );
        }

        public async Task<(List<CategoryResponseModel> Parents, List<CategoryResponseModel> Children)> GetParentAndChildCategoriesAsync()
        {
            var categories = await _apiRequestService.GetAllCategoriesAsync();
            var parents = categories.Where(c => c.ParentCategoryId == null).ToList();
            var children = categories.Where(c => c.ParentCategoryId != null).ToList();
            return (parents, children);
        }

        public async Task<List<CategoryResponseModel>> GetAllAsync()
        {
            return await _apiRequestService.GetAllCategoriesAsync();
        }

    }
}
