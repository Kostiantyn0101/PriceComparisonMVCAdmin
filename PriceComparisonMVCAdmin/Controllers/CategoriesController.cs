using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.Constants;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class CategoriesController : BaseController<CategoriesController>
    {
        private readonly IApiService _apiService;
        public CategoriesController(IApiService apiService, ILogger<CategoriesController> logger) : base(apiService, logger)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/Categories/getall");
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _apiService.GetAsync<CategoryResponseModel>($"api/Categories/{id}");
            if (category == null)
                return NotFound();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _apiService.PostAsync<CategoryCreateRequestModel, GeneralApiResponseModel>(
                "api/Categories/create", model);

            if (response.ReturnCode != AppSuccessCodes.CreateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                ModelState.AddModelError("", response.Message);
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _apiService.GetAsync<CategoryResponseModel>($"api/Categories/{id}");
            if (category == null)
                return NotFound();

            var model = new CategoryUpdateRequestModel
            {
                Id = category.Id,
                Title = category.Title,
                ParentCategoryId = category.ParentCategoryId,
                DisplayOrder = category.DisplayOrder
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _apiService.SendAsync<CategoryUpdateRequestModel, GeneralApiResponseModel>(
                HttpMethod.Put, "api/Categories/update", model, useMultipartFormData: true);

            if (response.ReturnCode != AppSuccessCodes.CreateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                ModelState.AddModelError("", response.Message);
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.SendAsync<object, GeneralApiResponseModel>(
                HttpMethod.Delete, $"api/Categories/delete/{id}", null);
            if (response.ReturnCode != AppSuccessCodes.CreateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
