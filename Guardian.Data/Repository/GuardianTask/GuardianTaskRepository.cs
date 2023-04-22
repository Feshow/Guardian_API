using Guardian.Domain.Interfaces.IRepository.TaskGuardian;
using Guardian.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Guardian.Data.Repository.GuardianTask
{
    public class GuardianTaskRepository : Repository<GuardianTaskModel>, IGuardianTaskRepository
    {
        private readonly ApplicationDbContext _db;
        public GuardianTaskRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<GuardianTaskModel> UpdateAsync(GuardianTaskModel entity)
        {
            _db.GuardianTasks.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<GuardianTaskModel> UpdateInactivateAsync(GuardianTaskModel entity)
        {
            entity.Inativar(entity);
            _db.GuardianTasks.Update(entity);            
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

