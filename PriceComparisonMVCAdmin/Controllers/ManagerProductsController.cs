using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class ManagerProductsController : BaseController<ManagerProductsController>
    {
        private readonly IManagerProductsService _managerProductsService;
        private readonly IProductCharacteristicService _characteristicService;
        private readonly IApiRequestService _apiRequestService;
        public ManagerProductsController(
            IProductCharacteristicService characteristicService,
            IManagerProductsService managerProductsService,
            ILogger<ManagerProductsController> logger,
            IApiRequestService apiRequestService,
            IApiService apiService)
               : base(apiService, logger)
        {
            _managerProductsService = managerProductsService;
            _characteristicService = characteristicService;
            _apiRequestService = apiRequestService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _managerProductsService.GetModerationViewModelAsync());
        }

        // CreateBaseProduct
        [HttpGet]
        public async Task<IActionResult> CreateBase()
        {
            return View(await _managerProductsService.CreateBaseProductViewModelAsync(new BaseProductFormModel()));
        }

        // POST: CreateBaseProduct
        [HttpPost]
        public async Task<IActionResult> CreateBase(BaseProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var refreshedViewModel = await _managerProductsService.CreateBaseProductViewModelAsync(model.BaseProduct);
                return View(refreshedViewModel);
            }

            var (product, errorMessage) = await _managerProductsService.CreateBaseProductAsync(model.BaseProduct);

            if (product == null)
            {
                ModelState.AddModelError("", errorMessage ?? "Unknown error");
                var refreshedViewModel = await _managerProductsService.CreateBaseProductViewModelAsync(model.BaseProduct);
                return View(refreshedViewModel);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditBaseProduct", new { id = product.Id });
        }

        // GET: EditBaseProduct
        [HttpGet]
        public async Task<IActionResult> EditBaseProduct(int id)
        {
            var viewModel = await _managerProductsService.GetEditBaseProductViewModelAsync(id);
            if (viewModel == null)
            {
                _logger.LogWarning("Базовий продукт з ID = {Id} не знайдено або API повернув пустий об'єкт.", id);
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: EditBaseProduct
        [HttpPost]
        public async Task<IActionResult> EditBaseProduct(BaseProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var refreshedViewModel = await _managerProductsService.GetEditBaseProductViewModelAsync(model.BaseProduct.Id ?? 0);
                if (refreshedViewModel != null)
                {
                    refreshedViewModel = model;
                }
                return View(refreshedViewModel ?? model);
            }

            var (success, errorMessage) = await _managerProductsService.UpdateBaseProductAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", errorMessage ?? "Unknown error");
                var refreshedViewModel = await _managerProductsService.GetEditBaseProductViewModelAsync(model.BaseProduct.Id ?? 0);
                return View(refreshedViewModel ?? model);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditBaseProduct", new { id = model.BaseProduct.Id });
        }


        // AddCharacteristic
        [HttpGet]
        public async Task<IActionResult> EditCharacteristics(int baseProductId, int? productId)
        {
            var viewModel = await _managerProductsService.GetEditCharacteristicsViewModelAsync(baseProductId, productId);
            return View(viewModel);
        }

        


        // GET: CreateVariant
        [HttpGet]
        public async Task<IActionResult> CreateVariant(int baseProductId)
        {
            var viewModel = await _managerProductsService.GetCreateVariantViewModelAsync(baseProductId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant(CreateVariantViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var refreshedViewModel = await _managerProductsService.GetCreateVariantViewModelAsync(viewModel.ProductVariant.BaseProductId);
                return View(refreshedViewModel);
            }

            var (product, errorMessage) = await _managerProductsService.CreateProductVariantAsync(viewModel.ProductVariant);

            if (product == null)
            {
                ModelState.AddModelError("", errorMessage ?? "Unknown error");
                var refreshedViewModel = await _managerProductsService.GetCreateVariantViewModelAsync(viewModel.ProductVariant.BaseProductId);
                return View(refreshedViewModel);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditVariant", new { id = product.Id });
        }

        // GET: EditVariant
        [HttpGet]
        public async Task<IActionResult> EditVariant(int id)
        {
            var viewModel = await _managerProductsService.GetEditVariantViewModelAsync(id);
            if (viewModel == null)
            {
                _logger.LogWarning("Варіант продукту з ID = {Id} не знайдено.", id);
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditVariant(EditVariantViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var refreshedModel = await _managerProductsService.GetEditVariantViewModelAsync(model.ProductVariant.Id);
                return View(refreshedModel);
            }

            var response = await _apiRequestService.UpdateProductVariantAsync(model.ProductVariant);

            if (response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                ModelState.AddModelError("", response.Message);
                var refreshedModel = await _managerProductsService.GetEditVariantViewModelAsync(model.ProductVariant.Id);
                return View(refreshedModel);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditVariant", new { id = model.ProductVariant.Id });
        }



        // POST: DeleteVariantProduct
        [HttpPost]
        public async Task<IActionResult> DeleteVariant(int id)
        {
            var response = await _apiRequestService.DeleteProductVariantAsync(id);

            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess &&
                response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVariantByJS(int id)
        {
            var response = await _apiRequestService.DeleteProductVariantAsync(id);

            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess &&
                response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return Json(new { success = false, message = response.Message });
            }
            return Json(new { success = true });
        }

        // POST: DeleteBaseProduct
        [HttpPost]
        public async Task<IActionResult> DeleteBaseProduct(int id)
        {
            var response = await _apiRequestService.DeleteBaseProductAsync(id);

            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess &&
                response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                TempData["Error"] = "\r\nНе вдалося видалити базовий продукт.";
                _logger.LogError("Не вдалося видалити базовий продукт з ідентифікатором {Id}", id);
                return RedirectToAction("EditBaseProduct", new { id = id });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBaseByJS(int id)
        {
            var response = await _apiRequestService.DeleteBaseProductAsync(id);

            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess &&
                response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return Json(new { success = false, message = response.Message });
            }
            return Json(new { success = true });
        }


        // GET: IndexBaseProducts
        public async Task<IActionResult> IndexBaseProducts()
        {
            var groupedCategories = await _managerProductsService.GetGroupedCategoriesAsync();
            return View(groupedCategories);
        }

        [HttpGet("ManagerProducts/ProductGroup/bytype/{groupTypeId}")]
        public async Task<IActionResult> GetByGroupType(int groupTypeId)
        {
            var groups = await _apiRequestService.GetGroupsByTypeIdAsync(groupTypeId);
            return Json(groups);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacteristic([FromBody] ProductCharacteristicViewModel model)
        {
            var result = await _characteristicService.CreateCharacteristicAsync(model);
            if (!result.success)
            {
                return BadRequest(new { message = result.errorMessage });
            }
            return Ok(new { message = "Created successfully", updatedId = result.updatedId });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacteristic([FromBody] ProductCharacteristicViewModel model)
        {
            var result = await _characteristicService.UpdateCharacteristicAsync(model);
            if (!result.success)
            {
                return BadRequest(new { message = result.errorMessage });
            }
            return Ok(new { message = "Updated successfully", updatedId = result.updatedId });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCharacteristic(int id)
        {
            var result = await _characteristicService.DeleteCharacteristicAsync(id);
            if (!result.success)
            {
                return BadRequest(new { message = result.errorMessage });
            }
            return Ok(new { message = "Deleted successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetBaseProductsByCategory(int id)
        {
            var baseProducts = await _apiRequestService.GetBaseProductByCategoryIdAsync(id);
            return Json(baseProducts);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsVariantsByBaseProduct(int id)
        {
            var productVariants = await _apiRequestService.GetVariantsByBaseProductIdAsync(id);
            return Json(productVariants);
        }

        [HttpGet]
        public async Task<IActionResult> GetVariantsOnModeration()
        {
            var variants = await _apiRequestService.GetProductVariantsOnModerationAsync();
            return Json(variants);
        }

        [HttpPost]
        public async Task<IActionResult> ReassignVariantToBase(int variantId, int newBaseProductId)
        {
            var variant = await _apiRequestService.GetProductVariantByIdAsync(variantId);
            if (variant == null)
                return Json(new { success = false, message = "Продукт не знайдено." });

            var updateModel = new ProductUpdateRequestModel
            {
                Id = variant.Id,
                GTIN = variant.GTIN,
                UPC = variant.UPC,
                ModelNumber = variant.ModelNumber,
                IsUnderModeration = variant.IsUnderModeration,
                BaseProductId = newBaseProductId,
                ColorId = variant.ColorId,
                IsDefault = variant.IsDefault,
                ProductGroupId = variant.ProductGroup?.Id
            };

            var response = await _apiRequestService.UpdateProductVariantAsync(updateModel);

            return Json(new
            {
                success = response.ReturnCode == AppSuccessCodes.UpdateSuccess,
                message = response.Message
            });
        }

    }
}
