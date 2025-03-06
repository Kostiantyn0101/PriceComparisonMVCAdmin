using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVC.Models;
using PriceComparisonMVC.Models.Categories;
using PriceComparisonMVC.Models.Response;
using PriceComparisonMVC.Services;

namespace PriceComparisonMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IApiService _apiService;

        public CategoriesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/categories/getall");
                return View(categories);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<CategoryResponseModel>());
            }
        }

      
        
        public async Task<IActionResult> CategoryList(int id)
        {
            var categories = await _apiService.GetAsync<List<CategoryResponseModel>>("api/categories/getall");

            var subcategories = categories
                .Where(c => c.ParentCategoryId.HasValue && c.ParentCategoryId.Value == id)
                .ToList();

            var categoryListModels = new List<CategiryListModel>();

            foreach (var subcategory in subcategories)
            {
                var productsWrapper = await _apiService.GetAsync<ProductsResponseWrapper>(
                    $"api/Products/bycategory/{subcategory.Id}?page=1");
                var products = productsWrapper?.Data ?? new List<ProductToCategiriesListModel>();


                foreach (var product in products)
                {
                    try
                    {
                        var productImages = await _apiService.GetAsync<List<ProductImageModel>>($"api/ProductImage/{product.Id}");
                        var imageUrl = productImages?.FirstOrDefault()?.ImageUrl;
                        product.ImageUrl = imageUrl;
                    }
                    catch (Exception ex)
                    {
                        product.ImageUrl = null;
                    }
                }

                var topProducts = products.Take(4).ToList();

                var categoryListModelOne = new CategiryListModel
                {
                    ParentCategory = subcategory,
                    ProductToCategiries = topProducts
                };

                categoryListModels.Add(categoryListModelOne);
            }

            return View(categoryListModels);
        }


        public async Task<IActionResult> CategoryProductList(int id)
        {

            var productsWrapper = await _apiService.GetAsync<ProductsResponseWrapper>($"api/Products/bycategory/{id}?page=1");
            var products = productsWrapper?.Data ?? new List<ProductToCategiriesListModel>();

            foreach (var product in products)
            {
                try
                {
                    var productImages = await _apiService.GetAsync<List<ProductImageModel>>($"api/ProductImage/{product.Id}");
                    var imageUrl = productImages?.FirstOrDefault()?.ImageUrl;
                    product.ImageUrl = imageUrl;
                }
                catch (Exception ex)
                {
                    product.ImageUrl = null;
                }
            }



            var viewModels = new List<ProductWithCharacteristicsViewModel>();

            foreach (var product in products)
            {
                try
                {
                    var characteristicGroups = await _apiService.GetAsync<List<ProductCharacteristicGroupResponseModel>>(
                        $"api/ProductCharacteristics/short-grouped/{product.Id}");

                    viewModels.Add(new ProductWithCharacteristicsViewModel
                    {
                        Product = product,
                        CharacteristicGroups = characteristicGroups ?? new List<ProductCharacteristicGroupResponseModel>()
                    });
                }
                catch (Exception ex)
                {
                    viewModels.Add(new ProductWithCharacteristicsViewModel
                    {
                        Product = product,
                        CharacteristicGroups = new List<ProductCharacteristicGroupResponseModel>()
                    });
                }
            }
            return View(viewModels);
        }


        public async Task<IActionResult> ProductList(int id)
        {
            return View(id);
        }
    }

}
