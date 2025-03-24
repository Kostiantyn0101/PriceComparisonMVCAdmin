using AutoMapper;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;

namespace PriceComparisonMVCAdmin.Infrastructure
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<BaseProductFormModel, BaseProductCreateRequestModel>();
            CreateMap<BaseProductResponseModel, BaseProductFormModel>();
            CreateMap<BaseProductFormModel, BaseProductUpdateRequestModel>();

            CreateMap<ProductResponseModel, ProductUpdateRequestModel>();

            //CreateMap<BaseProductCreateRequestModel, BaseProductFormModel>();
        }
    }
}
