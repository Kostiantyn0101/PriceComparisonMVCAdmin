using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs;

namespace PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts
{
    public class EditCharacteristicsViewModel
    {
        public int BaseProductId { get; set; }
        public int CategoryId { get; set; }
        public int? ProductId { get; set; }
        public List<ProductCharacteristicViewModel> Characteristics { get; set; } = new();
        public List<CategoryCharacteristicResponseModel> CharacteristicsMeta { get; set; } = new();
    }

}
