using Guardian.Domain.DTO.LoginUser;
using Guardian.Domain.DTO.LoginUser.Registration;
using Guardian.Domain.Models;

namespace Guardian.Domain.Interfaces.IRepository.User
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
        Task<LocalUser> Register(RegistrationResquestDTO registrationResquest);
    }
}
