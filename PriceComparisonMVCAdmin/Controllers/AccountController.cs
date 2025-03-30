using Microsoft.AspNetCore.Mvc;
using PriceComparisonMVCAdmin.Models.Response;
using PriceComparisonMVCAdmin.Services;
using PriceComparisonMVCAdmin.Models.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

            var token = await _authService.LoginAsync(model);

            if (token == null)
            {
                ModelState.AddModelError(string.Empty, "Невірний логін чи пароль");
                return View(model);
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roles = jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "ManagerProducts");
            }
            else if (roles.Contains("Seller"))
            {
                return RedirectToAction("Index", "Seller");
            }

            return RedirectToAction("Index", "Seller");
        }


        [HttpPost]
        public IActionResult Logout()
        {
            _tokenManager.ClearToken();
            return RedirectToAction("Login", "Account");
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

        [HttpGet]
        public IActionResult NoAccess()
        {
            return View();
        }
    }
}
