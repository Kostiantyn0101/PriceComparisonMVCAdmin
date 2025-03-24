using PriceComparisonMVCAdmin.Models.DTOs;

namespace PriceComparisonMVCAdmin.Services
{
    public interface IProductCharacteristicService
    {
        Task<(bool success, int? updatedId, string? errorMessage)> CreateCharacteristicAsync(ProductCharacteristicViewModel model);
        Task<(bool success, int? updatedId, string? errorMessage)> UpdateCharacteristicAsync(ProductCharacteristicViewModel model);
        Task<(bool success, string? errorMessage)> DeleteCharacteristicAsync(int id);
    }

}
