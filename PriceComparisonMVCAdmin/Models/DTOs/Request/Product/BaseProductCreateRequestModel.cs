namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Product
{
    public class BaseProductCreateRequestModel
    {
        public string Brand { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsUnderModeration { get; set; }
        public int CategoryId { get; set; }
    }
}
