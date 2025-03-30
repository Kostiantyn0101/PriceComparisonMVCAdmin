using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.DTOs;
using PriceComparisonMVCAdmin.Services;

namespace PriceComparisonMVCAdmin.Controllers;

[Authorize(Policy = "AdminRights")]
public class ProductCharacteristicsController : Controller
{
    private readonly IProductCharacteristicService _characteristicService;

    public ProductCharacteristicsController(
        IProductCharacteristicService characteristicService)
    {
        _characteristicService = characteristicService;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int baseProductId, int? productId)
    {
        var viewModel = await _characteristicService.GetEditCharacteristicsViewModelAsync(baseProductId, productId);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCharacteristicViewModel model)
    {
        var result = await _characteristicService.CreateCharacteristicAsync(model);
        if (!result.success)
        {
            return BadRequest(new { message = result.errorMessage });
        }
        return Ok(new { message = "Created successfully", updatedId = result.updatedId });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductCharacteristicViewModel model)
    {
        var result = await _characteristicService.UpdateCharacteristicAsync(model);
        if (!result.success)
        {
            return BadRequest(new { message = result.errorMessage });
        }
        return Ok(new { message = "Updated successfully", updatedId = result.updatedId });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _characteristicService.DeleteCharacteristicAsync(id);
        if (!result.success)
        {
            return BadRequest(new { message = result.errorMessage });
        }
        return Ok(new { message = "Deleted successfully" });
    }
}
