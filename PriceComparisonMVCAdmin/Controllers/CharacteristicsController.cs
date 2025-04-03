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
using PriceComparisonMVCAdmin.Models.DTOs.Response.Characteristics;
using System.Reflection.PortableExecutable;

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

        public async Task<IActionResult> Create()
        {
            var groups = await _apiRequestService.GetAllCharacteristicGroupsAsync();
            var dataTypes = await _apiRequestService.GetCharacteristicDataTypesAsync();

            var viewModel = new CharacteristicCreateViewModel
            {
                Characteristic = new(),
                CharacteristicGroups = groups,
                DataTypes = dataTypes
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CharacteristicCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.CharacteristicGroups = await _apiRequestService.GetAllCharacteristicGroupsAsync();
                viewModel.DataTypes = await _apiRequestService.GetCharacteristicDataTypesAsync();
                return View(viewModel);
            }

            var response = await _apiRequestService.CreateCharacteristicAsync(viewModel.Characteristic);
            if (_validationProcessor.TryProcessErrors(response, ModelState) || response.ReturnCode != AppSuccessCodes.CreateSuccess || response.Data == null)
            {
                TempData["Error"] = response.Message + " Не вдалось створити.";
                viewModel.CharacteristicGroups = await _apiRequestService.GetAllCharacteristicGroupsAsync();
                viewModel.DataTypes = await _apiRequestService.GetCharacteristicDataTypesAsync();
                return View(viewModel);
            }

            var characteristic = _apiResponseDeserializerService.DeserializeData<CharacteristicResponseModel>(response);
            TempData["SuccessMessage"] = "Характеристика створена успішно!";
            return RedirectToAction("Edit", new { id = characteristic.Id });
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

            var viewModel = new CharacteristicUpdateViewModel
            {
                Characteristic = _mapper.Map<CharacteristicUpdateRequestModel>(characreristic),
                CharacteristicGroups = groups,
                DataTypes = dataTypes
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CharacteristicUpdateViewModel viewModel)
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
            var response = await _apiRequestService.DeleteCharacteristicAsync(id);
            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess)
            {
                TempData["Error"] = response.Message;
            }
            TempData["SuccessMessage"] = "Дані видалено.";
            return RedirectToAction("Index");
        }
    }
}
