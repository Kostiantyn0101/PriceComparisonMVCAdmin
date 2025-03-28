using AutoMapper;
using PriceComparisonMVCAdmin.Models.Constants;
using PriceComparisonMVCAdmin.Models.DTOs.Request.Seller;
using PriceComparisonMVCAdmin.Models.DTOs.Response;
using PriceComparisonMVCAdmin.Models.Request.Seller;
using PriceComparisonMVCAdmin.Models.ViewModels.Seller;
using PriceComparisonMVCAdmin.Services.ApiServices;
using PriceComparisonMVCAdmin.Services.Helper;
using System.Security.Claims;

namespace PriceComparisonMVCAdmin.Services
{
    public class SellerService : ISellerService
    {
        private readonly IApiRequestService _apiRequestService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public SellerService(IApiRequestService apiRequestService, 
            IMapper mapper, 
            ICategoryService categoryService)
        {
            _apiRequestService = apiRequestService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<ProductReferenceStatisticsViewModel> GetReferenceStatisticsAsync(DateTime startDate, DateTime endDate, ClaimsPrincipal user)
        {
            var model = new ProductReferenceStatisticsViewModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out var sellerId))
            {
                var seller = await _apiRequestService.GetSellerByUserIdAsync(sellerId);

                var requestModel = new ProductSellerReferenceClickStaisticRequestModel
                {
                    SellerId = seller.Id,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var response = await _apiRequestService.GetProductReferenceClickAsync(requestModel);
                model.Results = response;
            }

            return model;
        }

        public async Task<SellerEditViewModel?> GetSellerEditViewModelAsync(int id)
        {
            var sellerResponse = await _apiRequestService.GetSellerByIdAsync(id);
            if (sellerResponse == null)
                return null;

            return _mapper.Map<SellerEditViewModel>(sellerResponse);
        }

        public async Task<GeneralApiResponseModel?> UpdateSellerAsync(SellerEditViewModel model)
        {
            var originalSeller = await _apiRequestService.GetSellerByIdAsync(model.Id);
            if (originalSeller == null)
                return null;

            var updateModel = new SellerUpdateRequestModel
            {
                Id = originalSeller.Id,
                ApiKey = originalSeller.ApiKey, //dont update
                StoreName = model.StoreName,
                WebsiteUrl = model.WebsiteUrl,
                IsActive = originalSeller.IsActive, //dont update
                AccountBalance = originalSeller.AccountBalance, //dont update
                UserId = originalSeller.UserId,
                DeleteCurrentLogoImage = model.NewLogoImage != null, //if new image is uploaded, delete the old one
                NewLogoImage = model.NewLogoImage,
                PublishPriceList = model.PublishPriceList
            };

            return await _apiRequestService.UpdateSellerAsync(updateModel);
        }

        public async Task<(bool IsSuccess, string Message)> UploadPriceListAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return (false, "Будь ласка, виберіть файл для завантаження.");
            }

            var request = new SellerProductXmlRequestModel { PriceXML = file };
            var response = await _apiRequestService.UploadPriceListAsync(request);

            if (response.ReturnCode == AppSuccessCodes.GerneralSuccess)
            {
                return (true, "Файл успішно завантажено!");
            }

            return (false, $"Сталася помилка: {response.Message}");
        }

        public async Task<SellerAuctionRatesGroupedViewModel> GetAuctionRatesViewModelAsync(ClaimsPrincipal user)
        {
            var model = new SellerAuctionRatesGroupedViewModel();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var sellerId))
                return model;

            var seller = await _apiRequestService.GetSellerByUserIdAsync(sellerId);
            model.SellerId = seller.Id;

            var auctionRates = await _apiRequestService.GetAuctionClickRateAsync(seller.Id);

            var (parents, children) = await _categoryService.GetParentAndChildCategoriesAsync();

            foreach (var parent in parents)
            {
                var groupedChildren = children
                    .Where(c => c.ParentCategoryId == parent.Id)
                    .Select(cat =>
                    {
                        var rate = auctionRates.FirstOrDefault(r => r.CategoryId == cat.Id);
                        return new CategoryAuctionRateViewModel
                        {
                            CategoryId = cat.Id,
                            CategoryTitle = cat.Title,
                            AuctionClickRateId = rate?.Id,
                            AuctionClickRate = rate?.ClickRate ?? 0
                        };
                    }).ToList();

                if (groupedChildren.Any())
                {
                    model.GroupedCategories.Add(
                        new ParentCategoryViewModel { Id = parent.Id, Title = parent.Title },
                        groupedChildren
                    );
                }
            }

            return model;
        }
    }
}
