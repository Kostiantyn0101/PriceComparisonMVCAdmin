namespace PriceComparisonMVC.Services
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
    }
}
