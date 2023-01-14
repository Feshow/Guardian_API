using Guardian.Domain.Models;

namespace Guardian.Application.Interfaces.IRepository.Guardian
{
    public interface IGuardianRepository : IRepository<GuardianModel>
    {
        Task<GuardianModel> UpdateAsync(GuardianModel model);
    }
}
