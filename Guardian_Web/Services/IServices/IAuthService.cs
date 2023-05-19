using Guardian_Web.Models.DTO.LoginUser;
using Guardian_Web.Models.DTO.LoginUser.Registration;

namespace Guardian_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegistrationResquestDTO objToCreate);
    }
}
