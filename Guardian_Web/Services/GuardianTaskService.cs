using Guardian_Utility;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.Guardian;
using Guardian_Web.Models.DTO.GuardianTask;
using Guardian_Web.Services.IServices;

namespace Guardian_Web.Services
{
    public class GuardianTaskService : BaseService, IGuardianTaskService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;

        public GuardianTaskService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>("ServicesUrls:GuardianAPI");
        }

        public Task<T> CreateAsync<T>(GuardianCreateTaskDTO dto)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = apiUrl + "/api/TaskAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = apiUrl + "/api/TaskAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + "/api/TaskAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + "/api/TaskAPI/" + id
            });
        }

        public Task<T> UpdadeAsync<T>(GuardianUpdateTaskDTO dto)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = apiUrl + "/api/TaskAPI/" + dto.Id
            });
        }
    }
}
