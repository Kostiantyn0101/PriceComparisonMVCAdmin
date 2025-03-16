using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PriceComparisonMVCAdmin.Models.Response.Seller;
using PriceComparisonMVCAdmin.Services;
using System.Security.Claims;

public class BaseController : Controller
{
    private readonly IApiService _apiService;

    public BaseController(IApiService apiService)
    {
        _apiService = apiService;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await base.OnActionExecutionAsync(context, next);
        ViewBag.Username = HttpContext?.User?.Identity?.Name;
        if (ViewBag.Username != null)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    try
                    {
                        var seller = await _apiService.GetAsync<SellerResponseModel>($"api/Seller/getByUserId/{userId}");
                        ViewBag.Seller = seller;
                    }
                    catch
                    {
                        ViewBag.Seller = null;
                    }
                }
            }
        }


        await next();
    }
}
