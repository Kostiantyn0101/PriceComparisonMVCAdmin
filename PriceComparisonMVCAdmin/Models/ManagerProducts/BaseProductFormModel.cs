namespace PriceComparisonMVCAdmin.Models.ManagerProducts
{
    public class BaseProductFormModel
    {
        public int? Id { get; set; }
        public string Brand { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsUnderModeration { get; set; }
        public DateTime? AddedToDatabase { get; set; }
    }
}
