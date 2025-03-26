using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Services.Helper;
using PriceComparisonMVCAdmin.Services.ApiServices;
using AutoMapper;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class CategoriesController : BaseController<CategoriesController>
    {
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;
        private readonly IValidationErrorProcessor _validationProcessor;
        private readonly IApiRequestService _apiRequestService;
        private readonly IMapper _mapper;

        public CategoriesController(IApiService apiService,
            IMapper mapper,
            IApiRequestService apiRequestService,
            IValidationErrorProcessor validationProcessor,
            IApiResponseDeserializerService apiResponseDeserializerService,
            ILogger<CategoriesController> logger) : base(apiService, logger)
        {
            _apiResponseDeserializerService = apiResponseDeserializerService;
            _apiRequestService = apiRequestService;
            _validationProcessor = validationProcessor;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _apiRequestService.GetAllCategoriesAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateRequestModel model)
        {
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
                TempData["SuccessMessage"] = "Дані збережено успішно.";
                return RedirectToAction("Edit", new { id = category.Id });
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _apiRequestService.GetCategoryByIdAsync(id);
            if (category.Id == 0)
            {
                return NotFound();
            }

            var model = _mapper.Map<CategoryUpdateRequestModel>(category);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _apiRequestService.UpdateCategoryAsync(model);
            if (_validationProcessor.TryProcessErrors(response, ModelState) ||
                response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                TempData["Error"] = response.Message + "Не вдалось відредагувати";
                return View(model);
            }

            TempData["SuccessMessage"] = "Дані змінені успішно.";
            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiRequestService.DeleteCategoryAsync(id);
            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess)
            {
                TempData["Error"] = response.Message;
            }
            TempData["SuccessMessage"] = "Дані видалено.";
            return RedirectToAction("Index");
        }
    }
}
