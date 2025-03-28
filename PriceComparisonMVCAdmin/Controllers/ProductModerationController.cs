using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "AdminRights")]
    [Route("api/products/moderation")]
    public class ProductModerationController : Controller
    {
        IApiRequestService _apiRequestService;
        IVariantProductService _variantProductService;

        public ProductModerationController(IApiRequestService apiRequestService, 
            IVariantProductService variantProductService)
        {
            _apiRequestService = apiRequestService;
            _variantProductService = variantProductService;
        }

        [HttpGet("onmoderation")]
        public async Task<IActionResult> GetVariantsOnModeration()
        {
            var variants = await _apiRequestService.GetProductVariantsOnModerationAsync();
            return Json(variants);
        }

        [HttpPost("reassign")]
        public async Task<IActionResult> ReassignVariantToBase(int variantId, int newBaseProductId)
        {
            var result = await _variantProductService.ReassignVariantToBaseAsync(variantId, newBaseProductId);
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
