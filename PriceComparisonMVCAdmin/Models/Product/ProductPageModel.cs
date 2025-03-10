using PriceComparisonMVCAdmin.Models.Response;

namespace PriceComparisonMVCAdmin.Models.Product
{
    public class ProductPageModel
    {
        public List<ProductCharacteristicResponseModel> Characteristics { get; set; }
        public List<ProductImageModel> ProductImages { get; set; }
        public List<CategoryResponseModel> CategoryBreadcrumb { get; set; }
        public int CategoryId { get; set; }
        public List<ProductCharacteristicGroupResponseModel> ShortCharacteristics { get; set; }
        public List<FeedbackResponseModel> Feedbacks { get; set; }
        public List<RelatedProductModel> RelatedProducts { get; set; }
        public List<SellerProductDetailResponseModel> SellerProductDetails { get; set; } 
                public ProductResponseModel ProductResponseModel { get; set; }
    }
}
