namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Product
{
    public class ProductImageCreateRequestModel
    {
        public int ProductId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
