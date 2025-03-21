using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.ManagerProducts;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Services.Helper;
using System.Reflection;

namespace PriceComparisonMVCAdmin.Controllers
{
    public class ManagerProductsController : BaseController<ManagerProductsController>
    {
        private readonly IApiResponseDeserializerService _apiResponseDeserializerService;

        public ManagerProductsController(IApiService apiService,
            IApiResponseDeserializerService apiResponseDeserializerService,
            ILogger<ManagerProductsController> logger)
               : base(apiService, logger)
        {
            _apiResponseDeserializerService = apiResponseDeserializerService;
        }

        // CreateBaseProduct
        [HttpGet]
        public async Task<IActionResult> CreateBase()
        {
            var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/Categories/getall");
            var filteredCategories = categories?.Where(c => c.ParentCategoryId.HasValue).ToList() ?? new List<CategoryResponseModel>();
            ViewBag.Categories = filteredCategories ?? new List<CategoryResponseModel>();
            var model = new BaseProductFormModel();
            return View(model);
        }

        // POST: CreateBaseProduct
        [HttpPost]
        public async Task<IActionResult> CreateBase(BaseProductFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/Categories/getall");
                var filteredCategories = categories?.Where(c => c.ParentCategoryId.HasValue).ToList() ?? new List<CategoryResponseModel>();
                ViewBag.Categories = filteredCategories ?? new List<CategoryResponseModel>();
                return View(model);
            }

            var modelCreate = new BaseProductCreateRequestModel
            {
                Brand = model.Brand,
                Title = model.Title,
                Description = model.Description,
                IsUnderModeration = model.IsUnderModeration,
                CategoryId = model.CategoryId
            };

            var response = await _apiService.PostAsync<BaseProductCreateRequestModel, GeneralApiResponseModel>(
                "api/BaseProducts/create", modelCreate);

            var product = _apiResponseDeserializerService.DeserializeData<BaseProductResponseModel>(response);

            if (product == null)
            {
                return await ReturnWithError("Invalid response data.", modelCreate);
            }

            if (response.ReturnCode != AppSuccessCodes.CreateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return await ReturnWithError(response.Message, modelCreate);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            //return RedirectToAction("EditCharacteristics", new { baseProductId = product.Id, categoryId = modelCreate.CategoryId });
            return RedirectToAction("EditBaseProduct", new { id = product.Id });
        }

        // GET: EditBaseProduct
        [HttpGet]
        public async Task<IActionResult> EditBaseProduct(int Id)
        {
            var baseProduct = await _apiService.GetAsync<BaseProductResponseModel>($"api/BaseProducts/{Id}");
            if (baseProduct == null)
            {
                return NotFound();
            }

            var productVariants = await _apiService.GetAsync<List<ProductResponseModel>>($"/api/Products/bybaseproduct/{Id}");
            ViewBag.productVariants = productVariants ?? [];

            var productColors = await _apiService.GetAsync<List<ColorResponseModel>>("/api/ProductColor/getall");
            ViewBag.productColors = productColors ?? [];

            var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/Categories/getall");
            var filteredCategories = categories?.Where(c => c.ParentCategoryId.HasValue).ToList() ?? new List<CategoryResponseModel>();

            ViewBag.Categories = filteredCategories ?? [];


            var model = new BaseProductFormModel
            {
                Id = baseProduct.Id,
                Brand = baseProduct.Brand,
                Title = baseProduct.Title,
                Description = baseProduct.Description,
                IsUnderModeration = baseProduct.IsUnderModeration,
                AddedToDatabase = baseProduct.AddedToDatabase,
                CategoryId = baseProduct.CategoryId
            };

            return View(model);
        }

        // POST: EditBaseProduct
        [HttpPost]
        public async Task<IActionResult> EditBaseProduct(BaseProductFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/Categories/getall");
                var filteredCategories = categories?.Where(c => c.ParentCategoryId.HasValue).ToList() ?? new List<CategoryResponseModel>();

                ViewBag.Categories = filteredCategories ?? new List<CategoryResponseModel>();
                return View(model);
            }

            var modelUpdate = new BaseProductUpdateRequestModel
            {
                Id = model.Id!.Value,
                Brand = model.Brand,
                Title = model.Title,
                Description = model.Description,
                IsUnderModeration = model.IsUnderModeration,
                AddedToDatabase = model.AddedToDatabase!.Value,
                CategoryId = model.CategoryId
            };

            var response = await _apiService.SendAsync<BaseProductUpdateRequestModel, GeneralApiResponseModel>(
                HttpMethod.Put, "api/BaseProducts/update", modelUpdate, useMultipartFormData: false);

            if (response.ReturnCode != AppSuccessCodes.UpdateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                ModelState.AddModelError("", response.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditBaseProduct", new { id = model.Id });
        }

        // Addcharacteristic for baseproduct
        [HttpGet]
        public async Task<IActionResult> EditCharacteristics(int baseProductId, int categoryId, int? productId)
        {
            var characteristics = await _apiService.GetAsync<List<CategoryCharacteristicResponseModel>>(
                $"api/CategoryCharacteristics/{categoryId}");

            bool isVariant = productId.HasValue;

            List<ProductCharacteristicUpdateRequestModel> existingCharacteristics = new List<ProductCharacteristicUpdateRequestModel>();

            if (isVariant)
            {
                try
                {
                    existingCharacteristics = await _apiService.GetAsync<List<ProductCharacteristicUpdateRequestModel>>(
                        $"api/ProductCharacteristics/{productId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не вдалося отримати характеристики продукту з ідентифікатором {ProductId}", productId);
                }
            }

            var model = characteristics.Select(c =>
            {
                var existingCharacteristic = existingCharacteristics.FirstOrDefault(ec => ec.CharacteristicId == c.CharacteristicId);

                return new ProductCharacteristicViewModel
                {
                    Id = existingCharacteristic?.Id ?? 0,
                    BaseProductId = baseProductId,
                    ProductId = productId,
                    CharacteristicId = c.CharacteristicId,
                    ValueText = existingCharacteristic?.ValueText,
                    ValueNumber = existingCharacteristic?.ValueNumber,
                    ValueBoolean = existingCharacteristic?.ValueBoolean ?? false,
                    ValueDate = existingCharacteristic?.ValueDate
                };
            }).ToList();

            ViewBag.CharacteristicsMeta = characteristics;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacteristic([FromBody] ProductCharacteristicViewModel model)
        {

            var newModel = new ProductCharacteristicCreateRequestModel
            {
                BaseProductId = model.BaseProductId,
                ProductId = model.ProductId,
                CharacteristicId = model.CharacteristicId,
                ValueText = !string.IsNullOrEmpty(model.ValueText) ? model.ValueText : null,
                ValueNumber = model.ValueNumber.HasValue ? model.ValueNumber : null,
                ValueDate = model.ValueDate.HasValue ? model.ValueDate : null,
                ValueBoolean = (string.IsNullOrEmpty(model.ValueText) && !model.ValueNumber.HasValue && !model.ValueDate.HasValue)
                        ? model.ValueBoolean : null
            };

            var response = await _apiService.PostAsync<ProductCharacteristicCreateRequestModel, GeneralApiResponseModel>(
                "api/ProductCharacteristics/create", newModel);

            if (response.ReturnCode != AppSuccessCodes.CreateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return BadRequest(new { message = response.Message });
            }

            var updatedCharacteristic = _apiResponseDeserializerService.DeserializeData<ProductCharacteristicResponseModel>(response);
            return Ok(new { message = "Created successfully", updatedId = updatedCharacteristic?.Id });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacteristic([FromBody] ProductCharacteristicViewModel model)
        {
            var newModel = new ProductCharacteristicUpdateRequestModel
            {
                Id = model.Id,
                BaseProductId = model.BaseProductId,
                ProductId = model.ProductId,
                CharacteristicId = model.CharacteristicId,
                ValueText = !string.IsNullOrEmpty(model.ValueText) ? model.ValueText : null,
                ValueNumber = model.ValueNumber.HasValue ? model.ValueNumber : null,
                ValueDate = model.ValueDate.HasValue ? model.ValueDate : null,
                ValueBoolean = (string.IsNullOrEmpty(model.ValueText) && !model.ValueNumber.HasValue && !model.ValueDate.HasValue)
                        ? model.ValueBoolean : null
            };

            var response = await _apiService.SendAsync<ProductCharacteristicUpdateRequestModel, GeneralApiResponseModel>(
                HttpMethod.Put,
                "api/ProductCharacteristics/update",
                newModel
            );

            if (response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                return BadRequest(new { message = response.Message });
            }

            var updatedCharacteristic = _apiResponseDeserializerService.DeserializeData<ProductCharacteristicResponseModel>(response);
            return Ok(new { message = "Updated successfully", updatedId = updatedCharacteristic?.Id });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCharacteristic(int id)
        {
            var response = await _apiService.DeleteAsync<GeneralApiResponseModel>($"api/ProductCharacteristics/delete/{id}");

            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess)
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(new { message = "Deleted successfully" });
        }


        // GET: CreateVariant
        [HttpGet]
        public async Task<IActionResult> CreateVariant(int baseProductId)
        {
            await LoadViewBagsCreateVariantAsync(baseProductId);

            var baseProduct = await _apiService.GetAsync<BaseProductResponseModel>($"api/BaseProducts/{baseProductId}");
            ViewBag.baseProduct = baseProduct;

            var model = new ProductCreateRequestModel
            {
                BaseProductId = baseProductId
            };
            return View(model);
        }



        // POST: ManagerProducts/CreateVariant
        [HttpPost]
        public async Task<IActionResult> CreateVariant(ProductCreateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagsCreateVariantAsync(model.BaseProductId);

                return View(model);
            }

            var response = await _apiService.PostAsync<ProductCreateRequestModel, GeneralApiResponseModel>(
                "api/Products/create", model);
            var product = _apiResponseDeserializerService.DeserializeData<ProductResponseModel>(response);


            if (response.ReturnCode != AppSuccessCodes.CreateSuccess && response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                ModelState.AddModelError("", response.Message);

                await LoadViewBagsCreateVariantAsync(model.BaseProductId);

                return View(model);
            }

            TempData["SuccessMessage"] = "Дані збережено успішно.";
            return RedirectToAction("EditVariant", new { id = product.Id });
        }

        private async Task LoadViewBagsCreateVariantAsync(int? baseProductId)
        {
            var groupTypes = await _apiService.GetAsync<List<ProductGroupTypeResponseModel>>("api/ProductGroupType/getall");
            ViewBag.GroupTypes = groupTypes ?? new List<ProductGroupTypeResponseModel>();

            var colors = await _apiService.GetAsync<List<ColorResponseModel>>("api/ProductColor/getall");
            ViewBag.Colors = colors ?? new List<ColorResponseModel>();

            var baseProduct = await _apiService.GetAsync<BaseProductResponseModel>($"api/BaseProducts/{baseProductId}");
            ViewBag.baseProduct = baseProduct;
        }



        // GET: EditVariant
        [HttpGet]
        public async Task<IActionResult> EditVariant(int id)
        {
            var product = await _apiService.GetAsync<ProductResponseModel>($"api/Products/{id}");
            if (product == null)
            {
                return NotFound();
            }

            await LoadViewBagsCreateVariantAsync(product.BaseProductId);

            ViewBag.SelectedGroupTypeId = product.ProductGroup.ProductGroupTypeId;

            var model = new ProductUpdateRequestModel()
            {
                Id = product.Id,
                GTIN = product.GTIN,
                UPC = product.UPC,
                ModelNumber = product.ModelNumber,
                IsUnderModeration = product.IsUnderModeration,
                BaseProductId = product.BaseProductId,
                ColorId = product.ColorId,
                IsDefault = product.IsDefault,
                ProductGroupId = product.ProductGroup.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditVariant(ProductUpdateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagsCreateVariantAsync(model.BaseProductId);
                return View(model);
            }

            var response = await _apiService.SendAsync<ProductUpdateRequestModel, GeneralApiResponseModel>(
                HttpMethod.Put,
                "api/Products/update",
                model
            );

            if (response.ReturnCode != AppSuccessCodes.UpdateSuccess)
            {
                ModelState.AddModelError("", response.Message);
                await LoadViewBagsCreateVariantAsync(model.BaseProductId);
                return View(model);
            }

            return RedirectToAction("EditVariant", new { id = model.Id });
        }


        // POST: DeleteProduct
        [HttpPost]
        public async Task<IActionResult> DeleteVariant(int id)
        {
            var response = await _apiService.DeleteAsync<GeneralApiResponseModel>($"api/Products/delete/{id}");

            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess &&
                response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("IndexBaseProducts");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVariantByJS(int id)
        {
            var response = await _apiService.DeleteAsync<GeneralApiResponseModel>($"api/Products/delete/{id}");

            if (response.ReturnCode != AppSuccessCodes.DeleteSuccess &&
                response.ReturnCode != AppSuccessCodes.GerneralSuccess)
            {
                return Json(new { success = false, message = response.Message });
            }
            return Json(new { success = true });
        }

        // POST: DeleteProduct
        [HttpPost]
        public async Task<IActionResult> DeleteBaseProduct(int id)
        {
            var response = new GeneralApiResponseModel();
            try
            {
                response = await _apiService.DeleteAsync<GeneralApiResponseModel>($"api/BaseProducts/delete/{id}");
            }
            catch (Exception ex)
            {
                if (response.ReturnCode != AppSuccessCodes.DeleteSuccess &&
                    response.ReturnCode != AppSuccessCodes.GerneralSuccess)
                {
                    TempData["Error"] = "\r\nНе вдалося видалити базовий продукт.";
                }
                _logger.LogError(ex, "Не вдалося видалити базовий продукт з ідентифікатором {Id}", id);
                return RedirectToAction("EditBaseProduct", new { id = id });
            }

            return RedirectToAction("IndexBaseProducts");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBaseByJS(int id)
        {
            var response = await _apiService.DeleteAsync<GeneralApiResponseModel>($"api/BaseProducts/delete/{id}");

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
            // Отримуємо всі категорії
            var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/Categories/getall");

            if (categories == null || !categories.Any())
            {
                return View(new Dictionary<CategoryResponseModel, List<CategoryResponseModel>>());
            }

            var parentCategories = categories.Where(c => c.ParentCategoryId == null).ToList();

            var groupedCategories = parentCategories.ToDictionary(
                parent => parent,
                parent => categories.Where(c => c.ParentCategoryId == parent.Id).ToList()
            );

            return View(groupedCategories);
        }


        //helper method
        private async Task<IActionResult> ReturnWithError(string errorMessage, BaseProductCreateRequestModel model)
        {
            ModelState.AddModelError("", errorMessage);
            var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/Categories/getall");
            ViewBag.Categories = categories ?? new List<CategoryResponseModel>();
            return View(model);
        }


    }
}
