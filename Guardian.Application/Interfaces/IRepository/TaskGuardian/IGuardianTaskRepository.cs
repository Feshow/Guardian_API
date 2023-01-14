using Guardian.Domain.Models;

namespace Guardian.Application.Interfaces.IRepository.TaskGuardian
{
    public interface IGuardianTaskRepository : IRepository<GuardianTaskModel>
    {
        Task<GuardianTaskModel> UpdateAsync(GuardianTaskModel model);
        Task<GuardianTaskModel> UpdateInactivateAsync(GuardianTaskModel model);
    }
}
