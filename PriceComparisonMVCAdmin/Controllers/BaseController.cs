using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PriceComparisonMVCAdmin.Controllers;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.Response.Seller;
using PriceComparisonMVCAdmin.Services.ApiServices;
using System.Security.Claims;
using System.Text.Json;

public abstract class BaseController<T> : Controller
{
    protected readonly IApiService _apiService;
    protected readonly ILogger<T> _logger;

    protected BaseController(IApiService apiService, ILogger<T> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ViewBag.Username = HttpContext?.User?.Identity?.Name;

        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        if (ViewBag.Username != null && User.Identity?.IsAuthenticated && roles.Contains("Seller"))
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    if (ViewBag.Seller == null)
                    {
                        var seller = await _apiService.GetAsync<SellerResponseModel>($"api/Seller/getByUserId/{userId}");
                        ViewBag.Seller = seller;
                    }
                }
                catch
                {
                    ViewBag.Seller = null;
                    _logger.LogError("Seller not fount");
                }
            }
        }

        await next();
    }

    protected IActionResult HandleApiResponse(GeneralApiResponseModel response, string successRedirect, object? routeValues = null)
    {
        if (response.ReturnCode == AppSuccessCodes.CreateSuccess ||
            response.ReturnCode == AppSuccessCodes.UpdateSuccess ||
            response.ReturnCode == AppSuccessCodes.DeleteSuccess ||
            response.ReturnCode == AppSuccessCodes.GerneralSuccess)
        {
            TempData["SuccessMessage"] = string.IsNullOrWhiteSpace(response.Message)
                        ? "Успішно."
                        : response.Message;
            return RedirectToAction(successRedirect, routeValues);
        }

        TempData["Error"] = response.Message ?? "Сталася помилка.";
        return RedirectToAction(successRedirect, routeValues);
    }
}
