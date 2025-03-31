using AutoMapper;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Categoty;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Category;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Product;
using PriceComparisonMVCAdmin.Models.DTOs.Response.Seller;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.ViewModels.ManagerProducts;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using System.Reflection;

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

            CreateMap<CategoryResponseModel, CategoryUpdateRequestModel>()
                .ForMember(dest => dest.NewImage, opt => opt.Ignore())
                .ForMember(dest => dest.NewIcon, opt => opt.Ignore())
                .ForMember(dest => dest.DeleteCurrentImage, opt => opt.Ignore())
                .ForMember(dest => dest.DeleteCurrentIcon, opt => opt.Ignore());

            CreateMap<SellerResponseModel, SellerEditViewModel>()
                .ForMember(dest => dest.CurrentLogoImageUrl, opt => opt.MapFrom(src => src.LogoImageUrl))
                .ForMember(dest => dest.NewLogoImage, opt => opt.Ignore());
            CreateMap<SellerResponseModel, AdminSellerEditViewModel>()
                .ForMember(dest => dest.CurrentLogoImageUrl, opt => opt.MapFrom(src => src.LogoImageUrl))
                .ForMember(dest => dest.NewLogoImage, opt => opt.Ignore());

            CreateMap<SellerRequestResponseModel, SellerRequestUpdateRequestModel>();

            //CreateMap<BaseProductCreateRequestModel, BaseProductFormModel>();
        }
    }
}
