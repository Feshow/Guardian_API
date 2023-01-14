using Guardian.Domain.Models;
using System.Linq.Expressions;

namespace Guardian.Application.Interfaces.IRepository
{
    public interface IGuardianRepository : IRepository<GuardianModel>
    {
        Task<GuardianModel> UpdateAsync(GuardianModel modal);
    }
}
