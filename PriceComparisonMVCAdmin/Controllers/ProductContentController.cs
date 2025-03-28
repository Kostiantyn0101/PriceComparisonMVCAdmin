using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Services.Helper;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using Microsoft.AspNetCore.Authorization;
using PriceComparisonMVCAdmin.Models.ViewModels.ProductContent;
using PriceComparisonMVCAdmin.Services.ApiServices;
using PriceComparisonMVCAdmin.Services;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class ProductContentController : BaseController<ProductContentController>
    {
        IApiRequestService _apiRequestService;
        IVariantProductService _variantProductService;


        public ProductContentController(IApiService apiService,
            IApiRequestService apiRequestService,
            IVariantProductService variantProductService,
            IApiResponseDeserializerService apiResponseDeserializerService,
            ILogger<ProductContentController> logger)
            : base(apiService, logger)
        {
            _variantProductService = variantProductService;
            _apiRequestService = apiRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexBaseProduct(int id)
        {
            var baseProduct = await _apiRequestService.GetBaseProductByIdAsync(id);

            var model = new BaseProductContentViewModel
            {
                BaseProductId = id,
                BaseProductTitle = baseProduct.Title,
                Videos = await _apiRequestService.GetBaseProductVideosAsync(id),
                Reviews = await _apiRequestService.GetReviewAsync(id),
                Instructions = await _apiRequestService.GetIstructionAsync(id)
            };

            return View(model);
        }

        // POST: Create Video
        [HttpPost]
        public async Task<IActionResult> CreateVideo(ProductVideoCreateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join("; ", errors);
                return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
            }

            var response = await _apiRequestService.CreateBaseProductVideoAsync(model);

            return HandleApiResponse(response, "IndexBaseProduct", new { id = model.BaseProductId });
        }

        // POST: Delete Video
        [HttpPost]
        public async Task<IActionResult> DeleteVideo(int id, int baseProductId)
        {
            var response = await _apiRequestService.DeleteBaseProductVideoAsync(id);
            return HandleApiResponse(response, "IndexBaseProduct", new { id = baseProductId });
        }

        // POST: Create Review
        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewCreateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join("; ", errors);
                return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
            }

            var response = await _apiRequestService.CreateReviewAsync(model);
            return HandleApiResponse(response, "IndexBaseProduct", new { id = model.BaseProductId });
        }


        // POST: Delete Review
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id, int baseProductId)
        {
            var response = await _apiRequestService.DeleteReviewAsync(id);
            return HandleApiResponse(response, "IndexBaseProduct", new { id = baseProductId });
        }

        // POST: Create Instruction
        [HttpPost]
        public async Task<IActionResult> CreateInstruction(InstructionCreateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join("; ", errors);
                return RedirectToAction("IndexBaseProduct", new { id = model.BaseProductId });
            }

            var response = await _apiRequestService.CreateIstructionAsync(model);
            return HandleApiResponse(response, "IndexBaseProduct", new { id = model.BaseProductId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInstruction(int id, int baseProductId)
        {
            var response = await _apiRequestService.DeleteIstructionAsync(id);
            return HandleApiResponse(response, "IndexBaseProduct", new { id = baseProductId });
        }

        [HttpGet]
        public async Task<IActionResult> IndexVariantProduct(int id)
        {
            var product = await _apiRequestService.GetProductVariantByIdAsync(id);
            var baseProduct = await _apiRequestService.GetBaseProductByIdAsync(product.BaseProductId);

            var model = new VariantProductContentViewModel
            {
                ProductId = id,
                BaseProductId = product.BaseProductId,
                BaseProductTitle = baseProduct.Title,
                ProductVariantTitle = _variantProductService.GetVariantTitle(product),
                Images = await _apiRequestService.GetProductImagesAsync(id)
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
            return HandleApiResponse(response, "IndexVariantProduct", new { id = model.ProductId });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(ProductImageDeleteViewModel model)
        {
            var requestModel = new ProductImageDeleteRequestModel
            {
                ProductImageIds = model.ProductImageIds
            };

            var response = await _apiRequestService.DeleteProductImageAsync(requestModel);
            return HandleApiResponse(response, "IndexVariantProduct", new { id = model.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> SetPrimaryImage(ProductImageSetPrimaryViewModel model)
        {
            var requestModel = new ProductImageSetPrimaryRequestModel
            {
                ProductImageId = model.ProductImageId
            };

            var response = await _apiRequestService.SetPrimaryImageAsync(requestModel);
            return HandleApiResponse(response, "IndexVariantProduct", new { id = model.ProductId });
        }
    }
}
