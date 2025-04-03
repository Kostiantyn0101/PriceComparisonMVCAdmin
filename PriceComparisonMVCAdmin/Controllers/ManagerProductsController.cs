using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class ManagerProductsController : BaseController<ManagerProductsController>
    {
        private readonly IProductModerationService _productModerationService;
        private readonly IVariantProductService _variantProductService;
        private readonly IBaseProductService _baseProductService;
        private readonly IApiRequestService _apiRequestService;
        private readonly ICategoryService _categoryService;
        public ManagerProductsController(
            IProductModerationService productModerationService,
            IVariantProductService variantProductService,
            ILogger<ManagerProductsController> logger,
            IBaseProductService baseProductService,
            IApiRequestService apiRequestService,
            ICategoryService categoryService,
            IApiService apiService)
               : base(apiService, logger)
        {
            _productModerationService = productModerationService;
            _variantProductService = variantProductService;
            _baseProductService = baseProductService;
            _apiRequestService = apiRequestService;
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _productModerationService.GetModerationViewModelAsync());
        }

        // GET: IndexBaseProducts
        public async Task<IActionResult> IndexBaseProducts()
        {
            var groupedCategories = await _categoryService.GetGroupedCategoriesAsync();
            return View(groupedCategories);
        }

        // CreateBaseProduct
        [HttpGet]
        public async Task<IActionResult> CreateBase()
        {
            return View(await _baseProductService.CreateBaseProductViewModelAsync(new BaseProductFormModel()));
        }

        // POST: CreateBaseProduct
        [HttpPost]
        public async Task<IActionResult> CreateBase(BaseProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var refreshedViewModel = await _baseProductService.CreateBaseProductViewModelAsync(model.BaseProduct);
                return View(refreshedViewModel);
            }

            var (product, errorMessage) = await _baseProductService.CreateBaseProductAsync(model.BaseProduct);

            if (product == null)
            {
                ModelState.AddModelError("", errorMessage ?? "Unknown error");
                var refreshedViewModel = await _baseProductService.CreateBaseProductViewModelAsync(model.BaseProduct);
                return View(refreshedViewModel);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditBaseProduct", new { id = product.Id });
        }

        // GET: EditBaseProduct
        [HttpGet]
        public async Task<IActionResult> EditBaseProduct(int id)
        {
            var viewModel = await _baseProductService.GetEditBaseProductViewModelAsync(id);
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
                var refreshedViewModel = await _baseProductService.GetEditBaseProductViewModelAsync(model.BaseProduct.Id ?? 0);
                if (refreshedViewModel != null)
                {
                    refreshedViewModel = model;
                }
                return View(refreshedViewModel ?? model);
            }

            var (success, errorMessage) = await _baseProductService.UpdateBaseProductAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", errorMessage ?? "Unknown error");
                var refreshedViewModel = await _baseProductService.GetEditBaseProductViewModelAsync(model.BaseProduct.Id ?? 0);
                return View(refreshedViewModel ?? model);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditBaseProduct", new { id = model.BaseProduct.Id });
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

        // GET: CreateVariant
        [HttpGet]
        public async Task<IActionResult> CreateVariant(int baseProductId)
        {
            var viewModel = await _variantProductService.GetCreateVariantViewModelAsync(baseProductId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant(CreateVariantViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var refreshedViewModel = await _variantProductService.GetCreateVariantViewModelAsync(viewModel.ProductVariant.BaseProductId);
                return View(refreshedViewModel);
            }

            var (product, errorMessage) = await _variantProductService.CreateProductVariantAsync(viewModel.ProductVariant);

            if (product == null)
            {
                ModelState.AddModelError("", errorMessage ?? "Unknown error");
                var refreshedViewModel = await _variantProductService.GetCreateVariantViewModelAsync(viewModel.ProductVariant.BaseProductId);
                return View(refreshedViewModel);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditVariant", new { id = product.Id });
        }

        // GET: EditVariant
        [HttpGet]
        public async Task<IActionResult> EditVariant(int id)
        {
            var viewModel = await _variantProductService.GetEditVariantViewModelAsync(id);
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
                var refreshedModel = await _variantProductService.GetEditVariantViewModelAsync(model.ProductVariant.Id);
                return View(refreshedModel);
            }

            var response = await _apiRequestService.UpdateProductVariantAsync(model.ProductVariant);

            if (response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                ModelState.AddModelError("", response.Message);
                var refreshedModel = await _variantProductService.GetEditVariantViewModelAsync(model.ProductVariant.Id);
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
    }
}
