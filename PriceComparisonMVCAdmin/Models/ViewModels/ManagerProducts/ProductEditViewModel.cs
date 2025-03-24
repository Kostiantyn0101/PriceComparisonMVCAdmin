using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;

namespace PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts
{
    public class ProductEditViewModel
    {
        public ProductResponseModel Product { get; set; }
        public List<ProductCharacteristicGroupResponseModel> CharacteristicGroups { get; set; }
    }

}
