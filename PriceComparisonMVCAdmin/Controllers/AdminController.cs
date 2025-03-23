using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Services;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class AdminController : BaseController<AdminController>
    {
        private readonly IApiService _apiService;

        public AdminController(IApiService apiService, ILogger<AdminController> logger) : base(apiService, logger)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
