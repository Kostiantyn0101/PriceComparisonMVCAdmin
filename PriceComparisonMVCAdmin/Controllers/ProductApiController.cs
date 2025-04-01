using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    [Route("api/products")]
    public class ProductApiController : BaseController<ProductApiController>
    {
        private readonly IApiRequestService _apiRequestService;
        public ProductApiController(IApiRequestService apiRequestService, 
            IApiService apiService, 
            ILogger<ProductApiController> logger) : base(apiService, logger)
        {
            _apiRequestService = apiRequestService;
        }

        [HttpGet("productgroup/bytype/{groupTypeId}")]
        public async Task<IActionResult> GetByGroupType(int groupTypeId)
        {
            var groups = await _apiRequestService.GetGroupsByTypeIdAsync(groupTypeId);
            return Json(groups);
        }

        [HttpGet("getbaseproduct/bycategory/{categoryId}")]
        public async Task<IActionResult> GetBaseProductsByCategory(int categoryId)
        {
            var baseProducts = await _apiRequestService.GetBaseProductByCategoryIdAsync(categoryId);
            return Json(baseProducts);
        }

        [HttpGet("variants/bybase/{id}")]
        public async Task<IActionResult> GetProductsVariantsByBaseProduct(int id)
        {
            var productVariants = await _apiRequestService.GetVariantsByBaseProductIdAsync(id);
            return Json(productVariants);
        }

        [HttpPost("deletebase")]
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

        [HttpPost("deletevariant")]
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
    }
}
