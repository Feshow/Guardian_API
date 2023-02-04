using Guardian_Web.Models.DTO.Guardian;

namespace Guardian_Web.Services.IServices
{
    public interface IGuardianService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(GuardianCreateDTO dto);
        Task<T> UpdadeAsync<T>(GuardianUpdateDTO dto);
        Task DeleteAsync<T>(int id);
    }
}
