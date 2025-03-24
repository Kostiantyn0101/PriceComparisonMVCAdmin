using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;

namespace PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts
{
    public class BaseProductViewModel
    {
        public BaseProductFormModel BaseProduct { get; set; }
        public List<CategoryResponseModel> Categories { get; set; } = new();
        public List<ProductResponseModel> productVariants { get; set; } = new();
        public List<ColorResponseModel> productColors { get; set; } = new();
    }
}
