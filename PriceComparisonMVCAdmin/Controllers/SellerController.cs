using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Services.ApiServices;
using System.Security.Claims;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "SellerRights")]
    public class SellerController : BaseController<SellerController>
    {
        IApiRequestService _apiRequestService;
        ISellerService _sellerService;

        public SellerController(IApiService apiService,
            IApiRequestService apiRequestService,
            ISellerService sellerService,
            ILogger<SellerController> logger)
               : base(apiService, logger)
        {
            _apiRequestService = apiRequestService;
            _sellerService = sellerService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _sellerService.GetReferenceStatisticsAsync(DateTime.Today.AddMonths(-1), DateTime.Today, User);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductReferenceStatisticsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultModel = await _sellerService.GetReferenceStatisticsAsync(model.StartDate, model.EndDate, User);

            return View(resultModel);
        }

        public async Task<IActionResult> Edit(int id)
        {

            var model = await _sellerService.GetSellerEditViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(SellerEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _sellerService.UpdateSellerAsync(model);
            if (response == null)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Оновлення успішне.";
            return RedirectToAction("Settings");
        }

        public async Task<IActionResult> Settings()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userId, out var sellerId))
            {
                var seller = await _apiRequestService.GetSellerByUserIdAsync(sellerId);
                return View(seller);
            }
            TempData["Error"] = "Invalid user ID";
            return RedirectToAction("Settings");
        }

        [HttpGet]
        public IActionResult UploadPrice()
        {
            return View(new PriceListViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> UploadPrice(PriceListViewModel model)
        {
            var (isSuccess, message) = await _sellerService.UploadPriceListAsync(model.PriceListFile);

            model.IsSuccess = isSuccess;
            model.Message = message;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            var model = await _sellerService.GetReferenceStatisticsAsync(DateTime.Today.AddMonths(-1), DateTime.Today, User);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Statistics(ProductReferenceStatisticsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultModel = await _sellerService.GetReferenceStatisticsAsync(model.StartDate, model.EndDate, User);

            return View(resultModel);
        }

        public async Task<IActionResult> AuctionRates()
        {
            var model = await _sellerService.GetAuctionRatesViewModelAsync(User);
            return View(model);
        }
    }
}
