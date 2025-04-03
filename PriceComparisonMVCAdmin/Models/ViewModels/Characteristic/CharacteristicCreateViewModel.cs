using PriceComparisonMVCAdmin.Models.DTOs.Request.Characteristic;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Characteristics;

namespace PriceComparisonMVCAdmin.Models.ViewModels.Characteristic
{
    public class CharacteristicCreateViewModel
    {
        public CharacteristicCreateRequestModel Characteristic { get; set; } = new();
        public List<CharacteristicGroupResponseModel> CharacteristicGroups { get; set; } = new();
        public List<string> DataTypes { get; set; } = new();
    }
}
