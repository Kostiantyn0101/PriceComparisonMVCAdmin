namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Product
{
    public class ProductVideoUpdateRequestModel
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public string VideoUrl { get; set; }
    }
}
