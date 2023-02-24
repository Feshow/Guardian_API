using Guardian_Web.Models.DTO.GuardianTask;

namespace Guardian_Web.Services.IServices
{
    public interface IGuardianTaskService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(GuardianCreateTaskDTO dto);
        Task<T> UpdadeAsync<T>(GuardianUpdateTaskDTO dto);
        Task<T>DeleteAsync<T>(int id);
    }
}
