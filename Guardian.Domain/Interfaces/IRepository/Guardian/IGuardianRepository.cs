using Guardian.Domain.Models;

namespace Guardian.Domain.Interfaces.IRepository.Guardian
{
    public interface IGuardianRepository : IRepository<GuardianModel>
    {
        Task<GuardianModel> UpdateAsync(GuardianModel model);
    }
}
