using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.Response;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Models.Request;

namespace PriceComparisonMVCAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly TokenManager _tokenManager;


        public AccountController(IAuthService authService, TokenManager tokenManager)
        {
            _authService = authService;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginResponseModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginResponseModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isSuccess = await _authService.LoginAsync(model);

            if (!isSuccess)
            {
                ModelState.AddModelError(string.Empty, "Невірний логін чи пароль");
                return View(model);
            }

            return RedirectToAction("Index", "Seller");
        }


        [HttpPost]
        public IActionResult Logout()
        {
            _tokenManager.ClearToken();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var errorMessage = await _authService.RegisterAsync(model);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }



    }
}
