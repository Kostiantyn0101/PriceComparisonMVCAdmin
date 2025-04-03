namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Characteristic
{
    public class CharacteristicCreateRequestModel
    {
        public string Title { get; set; }
        public string DataType { get; set; }
        public string? Unit { get; set; }
        public int CharacteristicGroupId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IncludeInShortDescription { get; set; }
    }
}
