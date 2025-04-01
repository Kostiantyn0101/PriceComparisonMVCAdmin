using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Services.Helper;
using PriceComparisonMVCAdmin.Services.ApiServices;
using AutoMapper;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Models.ViewModels.Category;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class CategoriesController : BaseController<CategoriesController>
    {
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;
        private readonly IValidationErrorProcessor _validationProcessor;
        private readonly IApiRequestService _apiRequestService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IApiService apiService,
            IMapper mapper,
            ICategoryService categoryService,
            IApiRequestService apiRequestService,
            IValidationErrorProcessor validationProcessor,
            IApiResponseDeserializerService apiResponseDeserializerService,
            ILogger<CategoriesController> logger) : base(apiService, logger)
        {
            _apiResponseDeserializerService = apiResponseDeserializerService;
            _apiRequestService = apiRequestService;
            _validationProcessor = validationProcessor;
            _mapper = mapper;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var groupedCategories = await _categoryService.GetGroupedCategoriesAsync();
            return View(groupedCategories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var (parents, _) = await _categoryService.GetParentAndChildCategoriesAsync();
            ViewBag.Parents = parents;

            return View(new CategoryCreateRequestModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateRequestModel model)
        {
            var (parents, _) = await _categoryService.GetParentAndChildCategoriesAsync();
            ViewBag.Parents = parents;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _apiRequestService.CreateCategoryAsync(model);

            if (_validationProcessor.TryProcessErrors(response, ModelState))
            {
                return View(model);
            }

            if (response.Data != null)
            {
                var category = _apiResponseDeserializerService.DeserializeData<CategoryResponseModel>(response);
                TempData["SuccessMessage"] = "Категорія створена успішно!";
                return RedirectToAction("Edit", new { id = category.Id });
            }

            TempData["Error"] = "Сталася помилка при створенні категорії.";
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _apiRequestService.GetCategoryByIdAsync(id);
            if (category == null || category.Id == 0)
            {
                return NotFound();
            }

            var (parents, _) = await _categoryService.GetParentAndChildCategoriesAsync();

            var viewModel = new CategoryEditViewModel
            {
                Category = _mapper.Map<CategoryUpdateRequestModel>(category),
                Parents = parents
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var (parents, _) = await _categoryService.GetParentAndChildCategoriesAsync();
                viewModel.Parents = parents;
                return View(viewModel);
            }

            var response = await _apiRequestService.UpdateCategoryAsync(viewModel.Category);
            if (_validationProcessor.TryProcessErrors(response, ModelState) || response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                TempData["Error"] = response.Message + " Не вдалось відредагувати.";
                var (parents, _) = await _categoryService.GetParentAndChildCategoriesAsync();
                viewModel.Parents = parents;
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Категорію оновлено успішно.";
            return RedirectToAction("Edit", new { id = viewModel.Category.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiRequestService.DeleteCategoryAsync(id);
            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess)
            {
                TempData["Error"] = response?.Message ?? "Помилка видалення категорії";
                return RedirectToAction("Edit", new { id });
            }

            TempData["SuccessMessage"] = "Категорію успішно видалено.";
            return RedirectToAction("Index");
        }
    }
}
