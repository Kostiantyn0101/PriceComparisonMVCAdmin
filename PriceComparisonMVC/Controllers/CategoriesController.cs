using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVC.Models.Response;
using PriceComparisonMVC.Services;

namespace PriceComparisonMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IApiService _apiService;

        public CategoriesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/categories/getall");
                return View(categories);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<CategoryResponseModel>());
            }
        }
    }

}
