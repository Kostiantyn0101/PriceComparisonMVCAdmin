using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Services.Helper;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using PriceComparisonMVCAdmin.Models.ViewModels.ProductContent;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class ProductContentController : BaseController<ProductContentController>
    {
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;

        public ProductContentController(IApiService apiService,
            IApiResponseDeserializerService apiResponseDeserializerService,
            ILogger<ProductContentController> logger)
            : base(apiService, logger)
        {
            _apiResponseDeserializerService = apiResponseDeserializerService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexBaseProduct(int id)
        {
            var videos = new List<BaseProductVideoResponseModel>();
            var reviews = new List<ReviewResponseModel>();
            var instructions = new List<InstructionResponseModel>();

            try
            {
                videos = await _apiService.GetAsync<List<BaseProductVideoResponseModel>>($"api/ProductVideo/{id}")
                         ?? new List<BaseProductVideoResponseModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка завантаження відео для продукту {Id}", id);
            }

            try
            {
                reviews = await _apiService.GetAsync<List<ReviewResponseModel>>($"api/Reviews/{id}")
                        ?? new List<ReviewResponseModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка завантаження оглядів для продукту {Id}", id);
            }

            try
            {
                instructions = await _apiService.GetAsync<List<InstructionResponseModel>>($"api/Instruction/{id}")
                             ?? new List<InstructionResponseModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка завантаження інструкцій для продукту {Id}", id);
            }

            var model = new BaseProductContentViewModel
            {
                BaseProductId = id,
                Videos = videos,
                Reviews = reviews,
                Instructions = instructions,
            };

            return View(model);
        }

        // POST: Create Video
        [HttpPost]
        public async Task<IActionResult> CreateVideo(ProductVideoCreateRequestModel model)
        {
            var response = await _apiService.PostAsync<ProductVideoCreateRequestModel, GeneralApiResponseModel>("api/ProductVideo/create", model);
            if (response.ReturnCode == AppSuccessCodes.CreateSuccess)
            {
                TempData["SuccessMessage"] = "Відео додано успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
        }

        // POST: Delete Video
        [HttpPost]
        public async Task<IActionResult> DeleteVideo(int id, int baseProductId)
        {
            var response = await _apiService.DeleteAsync<object, GeneralApiResponseModel>($"api/ProductVideo/delete/{id}");
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
            var response = await _apiService.PostAsync<ReviewCreateRequestModel, GeneralApiResponseModel>("api/Reviews/create", model);
            if (response.ReturnCode == AppSuccessCodes.CreateSuccess)
            {
                TempData["SuccessMessage"] = "Огляд додано успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
        }

        // POST: Delete Review
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id, int baseProductId)
        {
            var response = await _apiService.DeleteAsync<object, GeneralApiResponseModel>($"api/Reviews/delete/{id}");
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
            var response = await _apiService.PostAsync<InstructionCreateRequestModel, GeneralApiResponseModel>("api/Instruction/create", model);
            if (response.ReturnCode == AppSuccessCodes.CreateSuccess)
            {
                TempData["SuccessMessage"] = "Інструкцію додано успішно.";
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInstruction(int id, int baseProductId)
        {
            var response = await _apiService.DeleteAsync<object, GeneralApiResponseModel>($"api/Instruction/delete/{id}");
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
            var images = new List<ProductImageResponseModel>();

            try
            {
                images = await _apiService.GetAsync<List<ProductImageResponseModel>>($"api/ProductImage/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка завантаження зображень для продукту {Id}", id);
            }

            var model = new VariantProductContentViewModel
            {
                ProductId = id,
                Images = images,
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

            var response = await _apiService.SendAsync<ProductImageCreateRequestModel, GeneralApiResponseModel>(
                HttpMethod.Post, "api/ProductImage/add", model, useMultipartFormData: true);

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
            var response = await _apiService.DeleteAsync<ProductImageDeleteRequestModel, GeneralApiResponseModel>(
                "api/ProductImage/delete", requestModel);

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
            var response = await _apiService.SendAsync<ProductImageSetPrimaryRequestModel, GeneralApiResponseModel>(
                HttpMethod.Put, "api/ProductImage/setprimary", requestModel, useMultipartFormData: false);
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
