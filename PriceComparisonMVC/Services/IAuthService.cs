using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PriceComparisonMVC.Models.Response;

namespace PriceComparisonMVC.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginResponseModel login);
    }
}
