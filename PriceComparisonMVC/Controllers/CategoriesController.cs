using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVC.Models;
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
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiJiMmY4ZGEzYS0zOTYzLTQ5NjItODViMS1mNTA4OGEzMWI0NjYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbkBlbWFpbC5tZSIsImp0aSI6IjRlMTE1OGQxLTdhOTQtNDUwZS05MzZiLWM1NGQxYTEyYjczZCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJleHAiOjE3MzgwODUzNjksImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMDkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDA5In0.ANY7Q7IPe08AJ1Tcf4dF-Lr9TmyvUC0E7j9GdTjJ824";
            
            _apiService.SetAuthorizationHeader(jwtToken);

            try
            {
                var categories = await _apiService.GetAsync<List<CategoryDto>>("api/categories/getall");
                return View(categories);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<CategoryDto>());
            }
        }
    }

}
