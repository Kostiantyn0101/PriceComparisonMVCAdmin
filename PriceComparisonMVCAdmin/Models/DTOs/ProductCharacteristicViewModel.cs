namespace PriceComparisonMVCAdmin.Models.DTOs
{
    public class ProductCharacteristicViewModel
    {
        public int? Id { get; set; }
        public int? BaseProductId { get; set; }
        public int? ProductId { get; set; }
        public int CharacteristicId { get; set; }
        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public bool ValueBoolean { get; set; }
        public DateTime? ValueDate { get; set; }
    }
}
