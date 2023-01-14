using Guardian.Application.Interfaces.IRepository;
using Guardian.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Guardian.Data.Repository
{
    public class GuardianRepository : Repository<GuardianModel>, IGuardianRepository
    {
        private readonly ApplicationDbContext _db;
        public GuardianRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<GuardianModel> UpdateAsync(GuardianModel entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Guardians.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
