using Guardian_Utility;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.Guardian;
using Guardian_Web.Services.IServices;

namespace Guardian_Web.Services
{
    public class GuardianService : BaseService, IGuardianService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;

        public GuardianService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>("ServicesUrls:GuardianAPI");
        }

        public Task<T> CreateAsync<T>(GuardianCreateDTO dto)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType=SD.ApiType.POST,
                Data=dto,
                Url=apiUrl+ "/api/guardianAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = apiUrl + "/api/guardianAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + "/api/guardianAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + "/api/guardianAPI/" + id
            });
        }

        public Task<T> UpdadeAsync<T>(GuardianUpdateDTO dto)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = apiUrl + "/api/guardianAPI/" + dto.Id
            });
        }
    }
}
