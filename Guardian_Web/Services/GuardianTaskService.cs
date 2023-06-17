using Guardian_Utility;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.GuardianTask;
using Guardian_Web.Services.IServices;
using System;

namespace Guardian_Web.Services
{
    public class GuardianTaskService : BaseService, IGuardianTaskService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private string version;

        public GuardianTaskService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>("ServicesUrls:GuardianAPI");
            version = "v1";
        }

        public Task<T> CreateAsync<T>(GuardianCreateTaskDTO dto, string token)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = apiUrl + $"/api/{version}/TaskAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = apiUrl + $"/api/{version}/TaskAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + $"/api/{version}/TaskAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + $"/api/{version}/TaskAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdadeAsync<T>(GuardianUpdateTaskDTO dto, string token)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = apiUrl + $"/api/{version}/TaskAPI/" + dto.Id,
                Token = token
            });
        }
    }
}
