namespace PriceComparisonMVCAdmin.Services
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest requestData);
        Task<TResponse> SendAsync<TRequest, TResponse>(
            HttpMethod method,
            string endpoint,
            TRequest? requestData = default,
            bool useMultipartFormData = false);
    }
}
