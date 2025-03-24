using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;

namespace PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts
{
    public class CreateVariantViewModel
    {
        public ProductCreateRequestModel ProductVariant { get; set; } = new();
        [ValidateNever]
        public List<ProductGroupTypeResponseModel> GroupTypes { get; set; } = new();
        [ValidateNever]
        public List<ColorResponseModel> Colors { get; set; } = new();
        [ValidateNever]
        public BaseProductResponseModel BaseProduct { get; set; } = new();
    }
}
