using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Auction;
using PriceComparisonMVCAdmin.Services.ApiServices;

namespace PriceComparisonMVCAdmin.Controllers
{
    [Authorize(Policy = "SellerRights")]
    [Route("Seller/AuctionRate")]
    public class AuctionRateController : Controller
    {
        private readonly IApiRequestService _apiRequestService;

        public AuctionRateController(IApiRequestService apiRequestService)
        {
            _apiRequestService = apiRequestService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AuctionClickRateRequestModel model)
        {
            var result = await _apiRequestService.CreateAuctionClickRateAsync(model);
            return Json(new { success = true, newId = result.Data });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] AuctionClickRateRequestModel model)
        {
            var result = await _apiRequestService.UpdateAuctionClickRateAsync(model);
            return Json(new { success = true });
        }
    }
}
