using Guardian_Web.Models.DTO.GuardianTask;

namespace Guardian_Web.Services.IServices
{
    public interface IGuardianTaskService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(GuardianCreateTaskDTO dto, string token);
        Task<T> UpdadeAsync<T>(GuardianUpdateTaskDTO dto, string token);
        Task<T>DeleteAsync<T>(int id, string token);
    }
}
