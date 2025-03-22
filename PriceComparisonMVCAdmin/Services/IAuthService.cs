using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PriceComparisonMVCAdmin.Models.Response;
using PriceComparisonMVCAdmin.Models.Request;

namespace PriceComparisonMVCAdmin.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginResponseModel login);

        Task<string?> RegisterAsync(RegisterModel model);

    }
}
