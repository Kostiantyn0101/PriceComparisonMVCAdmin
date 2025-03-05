using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PriceComparisonMVC.Models;
using PriceComparisonMVC.Models.Categories;
using PriceComparisonMVC.Models.Product;
using PriceComparisonMVC.Models.Response;
using PriceComparisonMVC.Services;
using System.Text.Json;

namespace PriceComparisonMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IApiService _apiService;

        public ProductController(IApiService apiService)
        {
            _apiService = apiService;
        }
        
        public async Task<IActionResult> Index(int id)
        {
            List<ProductCharacteristicResponseModel>? characteristics = null;
            List<ProductCharacteristicGroupResponseModel>? shortCharacteristics = null;
            List<ProductImageModel>? productImages = null;
            ProductResponseModel? productResponseModel = null;
            CategoryResponseModel? category = null;
            List<FeedbackResponseModel>? feedbacks = null;
            List<SellerProductDetailResponseModel>? sellerProductDetails = null;
            List<RelatedProductModel> relatedProducts = new List<RelatedProductModel>();
            List<CategoryResponseModel>? breadcrumb = null;
            int productCategoryId = 0;


            try
            {
                characteristics = await _apiService.GetAsync<List<ProductCharacteristicResponseModel>>($"api/ProductCharacteristics/{id}");

                shortCharacteristics = await _apiService.GetAsync<List<ProductCharacteristicGroupResponseModel>>($"/api/ProductCharacteristics/short-grouped/{id}");

                productImages = await _apiService.GetAsync<List<ProductImageModel>>($"api/ProductImage/{id}");
                
                productResponseModel = await _apiService.GetAsync<ProductResponseModel>($"api/Products/{id}");
                                
                productCategoryId = productResponseModel.CategoryId;

                category = await _apiService.GetAsync<CategoryResponseModel>($"api/Categories/{productCategoryId}");
                
                breadcrumb = await BuildBreadcrumbAsync(category);

                //feedbacks = await _apiService.GetAsync<List<FeedbackResponseModel>>($"api/Feedback/{id}");

                // Отримання відгуків
                try
                {
                    feedbacks = await _apiService.GetAsync<List<FeedbackResponseModel>>($"api/Feedback/{id}");

                    // Перевірка та обробка отриманих даних
                    if (feedbacks != null)
                    {
                        // Можемо додатково обробити відгуки, якщо потрібно
                        // Наприклад, перевірити, чи не пусті обов'язкові поля та заповнити їх значеннями за замовчуванням
                        foreach (var feedback in feedbacks)
                        {
                            // Перевірка UserId
                            //if (feedback.UserId))
                            //{
                            //    feedback.UserId = 3;
                            //}

                            // Перевірка FeedbackText
                            if (string.IsNullOrEmpty(feedback.FeedbackText))
                            {
                                feedback.FeedbackText = "Відгук без тексту";
                            }

                            // Перевірка Rating (якщо рейтинг < 1, встановлюємо 1)
                            if (feedback.Rating < 1)
                            {
                                feedback.Rating = 1;
                            }

                            // Перевірка CreatedAt (якщо дата не встановлена, використовуємо поточну)
                            if (feedback.CreatedAt == default)
                            {
                                feedback.CreatedAt = DateTime.Now;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Загальна обробка інших помилок
                    Console.WriteLine($"Загальна помилка при отриманні відгуків: {ex.Message}");
                }



                var productsWrapper = await _apiService.GetAsync<ProductsResponseWrapper>($"api/Products/bycategory/{productCategoryId}?page=1");
                
                var relatedProductsRaw = productsWrapper?.Data ?? new List<ProductToCategiriesListModel>();

                var relatedProductsFiltered = relatedProductsRaw.Where(p => p.Id != id).Take(4).ToList();

                //var sellerProductDetails = await _apiService.GetAsync<List<SellerProductDetailResponseModel>>($"api/SellerProductDetails/{id}");

                // Отримання даних продавців
                try
                {
                    sellerProductDetails = await _apiService.GetAsync<List<SellerProductDetailResponseModel>>($"api/SellerProductDetails/{id}");
                    if (sellerProductDetails != null)
                    {
                        // Додаткова перевірка та виправлення порожніх значень
                        foreach (var seller in sellerProductDetails)
                        {
                            if (string.IsNullOrEmpty(seller.StoreName))
                            {
                                seller.StoreName = "Невідомий магазин";
                            }
                            if (string.IsNullOrEmpty(seller.ProductStoreUrl))
                            {
                                seller.ProductStoreUrl = "#";
                            }
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка отримання SellerProductDetails: {ex.Message}");
                    // Залишаємо порожній список
                }



                relatedProducts = new List<RelatedProductModel>();

                foreach (var relatedProduct in relatedProductsFiltered)
                {
                    // Завантажуємо зображення для кожного пов'язаного товару
                    var imagesForRelated = await _apiService.GetAsync<List<ProductImageModel>>($"api/ProductImage/{relatedProduct.Id}");
                    var imageUrl = imagesForRelated?.FirstOrDefault()?.ImageUrl ?? "placeholder.jpg";

                    relatedProducts.Add(new RelatedProductModel
                    {
                        Id = relatedProduct.Id,
                        ProductName = relatedProduct.Title,  
                        ProductPrice = 25000,
                        ImageUrl = imageUrl,
                    });
                }


                var product = new ProductPageModel
                {
                    Characteristics = characteristics ?? new List<ProductCharacteristicResponseModel>(),
                    ShortCharacteristics = shortCharacteristics ?? new List<ProductCharacteristicGroupResponseModel>(),
                    ProductImages = productImages ?? new List<ProductImageModel>(),
                    CategoryBreadcrumb = breadcrumb ?? new List<CategoryResponseModel>(),
                    Feedbacks = feedbacks ?? new List<FeedbackResponseModel>(),
                    RelatedProducts = relatedProducts, // Вже ініціалізований і не може бути null
                    SellerProductDetails = sellerProductDetails ?? new List<SellerProductDetailResponseModel>(),
                    ProductResponseModel = productResponseModel ?? new ProductResponseModel
                    {
                        Title = "Інформація недоступна",
                        Description = "Опис товару недоступний"
                    },
                    CategoryId = productCategoryId
                };

                var a = 3;

                return View(product);
            }
            catch (Exception ex)
            {
                return View(new ProductPageModel());
            }
        }


        private async Task<List<CategoryResponseModel>> BuildBreadcrumbAsync(CategoryResponseModel category)
        {
            var breadcrumb = new List<CategoryResponseModel>();
            while (category != null)
            {
                breadcrumb.Insert(0, category);

                if (category.ParentCategoryId.HasValue)
                {
                    category = await _apiService.GetAsync<CategoryResponseModel>($"api/Categories/{category.ParentCategoryId.Value}");
                }
                else
                {
                    category = null;
                }
            }
            return breadcrumb;
        }

    }
}
