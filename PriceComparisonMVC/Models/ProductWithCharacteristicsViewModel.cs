namespace PriceComparisonMVC.Models
{
    public class ProductWithCharacteristicsViewModel
    {
        public Categories.ProductToCategiriesListModel Product { get; set; }
        public List<Response.ProductCharacteristicGroupResponseModel> CharacteristicGroups { get; set; }
    }
}
