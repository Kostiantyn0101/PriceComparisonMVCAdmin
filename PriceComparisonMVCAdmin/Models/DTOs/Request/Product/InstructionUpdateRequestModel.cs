namespace PriceComparisonMVCAdmin.Models.DTOs.Request.Product
{
    public class InstructionUpdateRequestModel
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public string InstructionUrl { get; set; }
    }
}
