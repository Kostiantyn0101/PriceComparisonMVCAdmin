using PriceComparisonMVCAdmin.Models.Request;
using PriceComparisonMVCAdmin.Models.Response;

namespace PriceComparisonMVCAdmin.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginResponseModel login);

        Task<string?> RegisterAsync(RegisterModel model);

    }
}
