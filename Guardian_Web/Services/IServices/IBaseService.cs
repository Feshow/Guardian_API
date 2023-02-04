using Guardian_Web.Models.API;

namespace Guardian_Web.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIResquest aPIResquest); //Send API calls to API
    }
}
