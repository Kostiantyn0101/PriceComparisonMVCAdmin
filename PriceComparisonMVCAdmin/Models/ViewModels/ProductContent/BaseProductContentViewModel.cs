using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;

namespace PriceComparisonMVCAdmin.Models.ViewModels.ProductContent
{
    public class BaseProductContentViewModel
    {
        public int BaseProductId { get; set; }
        public string BaseProductTitle { get; set; } = null!;
        public List<BaseProductVideoResponseModel> Videos { get; set; }
        public List<ReviewResponseModel> Reviews { get; set; }
        public List<InstructionResponseModel> Instructions { get; set; }
    }
}
