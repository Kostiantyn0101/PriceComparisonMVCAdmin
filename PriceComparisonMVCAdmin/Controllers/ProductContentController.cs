using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Services.Helper;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using Microsoft.AspNetCore.Authorization;
using PriceComparisonMVCAdmin.Models.ViewModels.ProductContent;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class ProductContentController : BaseController<ProductContentController>
    {
        IApiRequestService _apiRequestService;

        public ProductContentController(IApiService apiService,
            IApiRequestService apiRequestService,
            IApiResponseDeserializerService apiResponseDeserializerService,
            ILogger<ProductContentController> logger)
            : base(apiService, logger)
        {
            _apiRequestService = apiRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexBaseProduct(int id)
        {
            var model = new BaseProductContentViewModel
            {
                BaseProductId = id,
                Videos = await _apiRequestService.GetBaseProductVideosAsync(id),
                Reviews = await _apiRequestService.GetReviewAsync(id),
                Instructions = await _apiRequestService.GetIstructionAsync(id),
            };

            return View(model);
        }

        // POST: Create Video
        [HttpPost]
        public async Task<IActionResult> CreateVideo(ProductVideoCreateRequestModel model)
        {
            var response = await _apiRequestService.CreateBaseProductVideoAsync(model);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join("; ", errors);
                return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
            }
            if (response.ReturnCode == AppSuccessCodes.CreateSuccess)
            {
                TempData["SuccessMessage"] = "Відео додано успішно.";
            }

            return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
        }

        // POST: Delete Video
        [HttpPost]
        public async Task<IActionResult> DeleteVideo(int id, int baseProductId)
        {
            var response = await _apiRequestService.DeleteBaseProductVideoAsync(id);
            if (response.ReturnCode == AppSuccessCodes.DeleteSuccess)
            {
                TempData["SuccessMessage"] = "Відео видалено успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexBaseProduct", new { id = baseProductId });
        }

        // POST: Create Review
        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewCreateRequestModel model)
        {
            var response = await _apiRequestService.CreateReviewAsync(model);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join("; ", errors);
                return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
            }
            if (response.ReturnCode == AppSuccessCodes.CreateSuccess)
            {
                TempData["SuccessMessage"] = "Огляд додано успішно.";
            }
            return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
        }

        // POST: Delete Review
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id, int baseProductId)
        {
            var response = await _apiRequestService.DeleteReviewAsync(id);
            if (response.ReturnCode == AppSuccessCodes.DeleteSuccess)
            {
                TempData["SuccessMessage"] = "Огляд видалено успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexBaseProduct", new { id = baseProductId });
        }

        // POST: Create Instruction
        [HttpPost]
        public async Task<IActionResult> CreateInstruction(InstructionCreateRequestModel model)
        {
            var response = await _apiRequestService.CreateIstructionAsync(model);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join("; ", errors);
                return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
            }
            if (response.ReturnCode == AppSuccessCodes.CreateSuccess)
            {
                TempData["SuccessMessage"] = "Інструкцію додано успішно.";
            }

            return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInstruction(int id, int baseProductId)
        {
            var response = await _apiRequestService.DeleteIstructionAsync(id);
            if (response.ReturnCode == AppSuccessCodes.DeleteSuccess)
            {
                TempData["SuccessMessage"] = "Інструкцію видалено успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexBaseProduct", new { id = baseProductId });
        }


        [HttpGet]
        public async Task<IActionResult> IndexVariantProduct(int id)
        {
            var response = await _apiRequestService.GetProductImagesAsync(id);

            var model = new VariantProductContentViewModel
            {
                ProductId = id,
                Images = response,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProductImage(ProductImageCreateRequestModel model)
        {
            if (model.Images == null || !model.Images.Any())
            {
                TempData["Error"] = "Не вибрано зображення для завантаження.";
                return RedirectToAction("IndexVariantProduct", new { id = model.ProductId });
            }

            var response = await _apiRequestService.AddProductImageAsync(model);

            if (response.ReturnCode == AppSuccessCodes.CreateSuccess)
            {
                TempData["SuccessMessage"] = "Зображення додано успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexVariantProduct", new { id = model.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(ProductImageDeleteViewModel model)
        {
            var requestModel = new ProductImageDeleteRequestModel
            {
                ProductImageIds = model.ProductImageIds
            };
            var response = await _apiRequestService.DeleteProductImageAsync(requestModel);

            if (response.ReturnCode == AppSuccessCodes.DeleteSuccess)
            {
                TempData["SuccessMessage"] = "Зображення видалено успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexVariantProduct", new { id = model.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> SetPrimaryImage(ProductImageSetPrimaryViewModel model)
        {
            var requestModel = new ProductImageSetPrimaryRequestModel
            {
                ProductImageId = model.ProductImageId
            };

            var response = await _apiRequestService.SetPrimaryImageAsync(requestModel);

            if (response.ReturnCode == AppSuccessCodes.UpdateSuccess)
            {
                TempData["SuccessMessage"] = "Основне зображення встановлено успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexVariantProduct", new { id = model.ProductId });
        }
    }
}
