using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Services.Helper;
using PriceComparisonMVCAdmin.Services.ApiServices;
using AutoMapper;
using PriceComparisonMVCAdmin.Models.ViewModels.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Characteristic;
using PriceComparisonMVCAdmin.Models.ViewModels.Characteristic;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    public class CharacteristicsController : BaseController<CharacteristicsController>
    {
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;
        private readonly IValidationErrorProcessor _validationProcessor;
        private readonly IApiRequestService _apiRequestService;
        private readonly IMapper _mapper;

        public CharacteristicsController(IApiService apiService,
            IMapper mapper,
            IApiRequestService apiRequestService,
            IValidationErrorProcessor validationProcessor,
            IApiResponseDeserializerService apiResponseDeserializerService,
            ILogger<CharacteristicsController> logger) : base(apiService, logger)
        {
            _apiResponseDeserializerService = apiResponseDeserializerService;
            _apiRequestService = apiRequestService;
            _validationProcessor = validationProcessor;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var characteristrics = await _apiRequestService.GetAllCharacteristicsAsync();
            return View(characteristrics);
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
            var characreristic = await _apiRequestService.GetCharacteristicByIdAsync(id);
            if (characreristic == null || characreristic.Id == 0)
            {
                return NotFound();
            }

            var groups = await _apiRequestService.GetAllCharacteristicGroupsAsync();
            var dataTypes = await _apiRequestService.GetCharacteristicDataTypesAsync();

            var viewModel = new CharacteristicEditViewModel
            {
                Characteristic = _mapper.Map<CharacteristicUpdateRequestModel>(characreristic),
                CharacteristicGroups = groups,
                DataTypes = dataTypes
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CharacteristicEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.CharacteristicGroups = await _apiRequestService.GetAllCharacteristicGroupsAsync();
                viewModel.DataTypes = await _apiRequestService.GetCharacteristicDataTypesAsync();
                return View(viewModel);
            }

            var response = await _apiRequestService.UpdateCharacteristicAsync(viewModel.Characteristic);
            if (_validationProcessor.TryProcessErrors(response, ModelState) || response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                TempData["Error"] = response.Message + " Не вдалось відредагувати.";
                viewModel.CharacteristicGroups = await _apiRequestService.GetAllCharacteristicGroupsAsync();
                viewModel.DataTypes = await _apiRequestService.GetCharacteristicDataTypesAsync();
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Характеристику оновлено успішно.";
            return RedirectToAction("Edit", new { id = viewModel.Characteristic.Id });
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
