using Guardian_Utility;
using Guardian_Web.Models.API;
using Guardian_Web.Models.DTO.LoginUser;
using Guardian_Web.Models.DTO.LoginUser.Registration;
using Guardian_Web.Services.IServices;

namespace Guardian_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private string version;
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>("ServicesUrls:GuardianAPI");
            version = "v1";
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = apiUrl + $"/api/{version}/UsersAuth/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationResquestDTO obj)
        {
            return SendAsync<T>(new APIResquest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = apiUrl + $"/api/{version}/UsersAuth/register"
            });
        }
    }
}
