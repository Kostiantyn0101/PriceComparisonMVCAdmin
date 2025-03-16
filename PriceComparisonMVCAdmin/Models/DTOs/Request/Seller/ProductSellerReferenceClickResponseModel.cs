namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Seller
{
    public class ProductSellerReferenceClickResponseModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SellerProductReference { get; set; }
        public int SellerId { get; set; }
        public DateTime ClickedAt { get; set; }
        public decimal ClickRate { get; set; }
    }
}
