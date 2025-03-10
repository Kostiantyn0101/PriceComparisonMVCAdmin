using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models;
using PriceComparisonMVCAdmin.Models.Response;
using PriceComparisonMVCAdmin.Services;

namespace PriceComparisonMVCAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TokenManager _tokenManager;
        private readonly IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, TokenManager tokenManager, IApiService apiService)
        {
            _logger = logger;
            _tokenManager = tokenManager;
            _apiService = apiService;
        }

        public async Task<IActionResult> IndexAsync()
        {

            List<CategoryResponseModel> categoryResponses = await _apiService.GetAsync<List<CategoryResponseModel>>("api/categories/getall");

            var indexContent = Data.IndexContentData.GetIndexContent(categoryResponses);

            ViewBag.Username = HttpContext?.User?.Identity?.Name;
            return View(indexContent);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
