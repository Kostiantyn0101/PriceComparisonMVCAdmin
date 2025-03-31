using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class SellerRequestController : BaseController<SellerRequestController>
    {
        private readonly ISellerRequestService _sellerRequestService;
        public SellerRequestController(IApiService apiService, ISellerRequestService sellerRequestService,
            ILogger<SellerRequestController> logger) : base(apiService, logger)
        {
            _sellerRequestService = sellerRequestService;
        }

        public async Task<IActionResult> Index()
        {
            var sellerRequests = await _sellerRequestService.GetAllSellerRequests();
            return View(sellerRequests);
        }


        [HttpGet]
        public async Task<IActionResult> GetSellerRequestInfoPartial(int id)
        {
            var partialResponse = await _sellerRequestService.GetSellerRequestInfoPartialViewModelAsync(id);
            if (partialResponse == null)
            {
                return NotFound("Продавець не знайдений");
            }

            return PartialView("_SellerRequestInfoPartial", partialResponse);
        }

        [HttpGet] 
        public async Task<IActionResult> Edit(int id)
        {
            var sellerRequest = await _sellerRequestService.GetSellerRequestAsync(id);
            if (sellerRequest == null)
            {
                return NotFound("Продавець не знайдений");
            }
            return View(sellerRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SellerRequestResponseModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _sellerRequestService.UpdateSellerRequestAsync(model);
            if (response == null || response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                TempData["Error"] = "Сталась помилка при запиті.";
                return HandleApiResponse(response!, "Edit");
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Approve([FromBody] SellerRequestProcessRequestModel model)
        {
            var response = await _sellerRequestService.ApproveSellerRequest(model);
            if (response == null || response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                return BadRequest("Не вдалося обробити заявку.");
            }

            return Ok();
        }

    }
}
