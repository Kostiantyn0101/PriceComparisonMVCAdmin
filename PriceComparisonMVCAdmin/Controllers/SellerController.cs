using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.Response;
using PriceComparisonMVCAdmin.Models.Response.Seller;
using PriceComparisonMVCAdmin.Models.Seller;
using PriceComparisonMVCAdmin.Services;
using System.Security.Claims;
using System.Text.Json;

namespace PriceComparisonMVCAdmin.Controllers
{
    public class SellerController : BaseController
    {
        private readonly IApiService _apiService;

        public SellerController(IApiService apiService) : base(apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            SellerResponseModel? seller = null;

            try
            {
                seller = await _apiService.GetAsync<SellerResponseModel>($"api/Seller/getByUserId/{userId}");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Не вдалося отримати дані продавця";
                //redirect to error page or access deni
            }

            return View(seller);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sellerResponse = await _apiService.GetAsync<SellerResponseModel>($"api/Seller/{id}");
            if (sellerResponse == null)
            {
                return NotFound();
            }

            var model = new SellerEditViewModel
            {
                Id = sellerResponse.Id,
                StoreName = sellerResponse.StoreName,
                WebsiteUrl = sellerResponse.WebsiteUrl,
                CurrentLogoImageUrl = sellerResponse.LogoImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SellerEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var originalSeller = await _apiService.GetAsync<SellerResponseModel>($"api/Seller/{model.Id}");
            if (originalSeller == null)
            {
                return NotFound();
            }

            var updateModel = new SellerUpdateRequestModel
            {
                Id = originalSeller.Id,
                ApiKey = originalSeller.ApiKey, //dont update
                StoreName = model.StoreName,
                WebsiteUrl = model.WebsiteUrl,
                IsActive = originalSeller.IsActive, //dont update
                AccoundBalance = originalSeller.AccoundBalance, //dont update
                UserId = originalSeller.UserId,
                DeleteCurrentLogoImage = model.NewLogoImage != null, //if new image is uploaded, delete the old one
                NewLogoImage = model.NewLogoImage
            };

            try
            {
                var response = await _apiService.SendAsync<SellerUpdateRequestModel, GeneralApiResponseModel>(
                    HttpMethod.Put,
                    "api/Seller/update",
                    updateModel,
                    useMultipartFormData: true
                );
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Оновлення не вдалося";
                return View(model);
            }
        }
        public async Task<IActionResult> Settings()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            SellerResponseModel? seller = null;

            try
            {
                seller = await _apiService.GetAsync<SellerResponseModel>($"api/Seller/getByUserId/{userId}");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Не вдалося отримати дані продавця";
                //redirect to error page or access deni
            }

            return View(seller);
        }
    }
}
