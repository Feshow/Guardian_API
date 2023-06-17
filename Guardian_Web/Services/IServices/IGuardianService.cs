using Guardian_Web.Models.DTO.Guardian;

namespace Guardian_Web.Services.IServices
{
    public interface IGuardianService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(GuardianCreateDTO dto, string token);
        Task<T> UpdadeAsync<T>(GuardianUpdateDTO dto, string token);
        Task<T>DeleteAsync<T>(int id, string token);
    }
}
