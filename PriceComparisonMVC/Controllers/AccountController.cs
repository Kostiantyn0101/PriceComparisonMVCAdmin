using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVC.Models;
using PriceComparisonMVC.Models.Response;
using PriceComparisonMVC.Services;

namespace PriceComparisonMVC.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var login = new LoginResponseModel();
            login.Username = email;
            login.Password = password;
            var result = await _authService.LoginAsync(login);

            var breakpoint = result;
            //if (!result)
            //    return View("Error");

            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public IActionResult Register(string Name, string Password)
        {
            // Логіка авторизації

            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string Name, string Phone, string Email, string Password)
        {
            // Логіка реєстрації
           
            return RedirectToAction("Index", "Home");
        }
    }
}
