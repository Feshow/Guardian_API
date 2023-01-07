using Guardian.Domain.Models;
using System.Linq.Expressions;

namespace Guardian.Application.Interfaces.IRepository
{
    public interface IGuardianRepository
    {
        Task<List<GuardianModel>> GetAllAsync(Expression<Func<GuardianModel, bool>> filter = null);
        Task<GuardianModel> GetAsync(Expression<Func<GuardianModel, bool>> filter = null, bool tracked = true);
        Task CreateAsync(GuardianModel modal);
        Task RemoveAsync(GuardianModel modal);
        Task UpdateAsync(GuardianModel modal);
        Task SaveAsync();
    }
}
