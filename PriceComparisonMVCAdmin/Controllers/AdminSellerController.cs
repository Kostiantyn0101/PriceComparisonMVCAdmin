using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using PriceComparisonMVCAdmin.Services.ApiServices;
using PriceComparisonMVCAdmin.Services;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class AdminSellerController : BaseController<AdminSellerController>
    {
        private readonly ISellerService _sellerService;

        public AdminSellerController(ISellerService sellerService, 
            IApiService apiService, 
            ILogger<AdminSellerController> logger)
            : base(apiService, logger)
        {
            _sellerService = sellerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sellers = await _sellerService.GetAllSellersAsync();
            return View(sellers);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _sellerService.GetAdminSellerEditViewModelAsync(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminSellerEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _sellerService.UpdateAdminSellerAsync(model);
            if (response == null)
            {
                TempData["Error"] = "Сталась помилка при запиті.";
                return HandleApiResponse(response!, "Edit");
            }

            return HandleApiResponse(response, "Edit");
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminSellerInfoPartial(int id)
        {
            var partialResponse = await _sellerService.GetAdminSellerInfoPartialViewModelAsync(id);
            if (partialResponse == null)
            { 
                return NotFound("Продавець не знайдений");
            }    

            return PartialView("_SellerRequestInfoPartial", partialResponse);
        }
    }
}
