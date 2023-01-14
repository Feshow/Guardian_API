using Guardian.Application.Interfaces.IRepository.TaskGuardian;
using Guardian.Domain.Models;


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
            entity.UpdatedDate = DateTime.Now;
            _db.GuardianTasks.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<GuardianTaskModel> UpdateInactivateAsync(GuardianTaskModel entity)
        {
            entity.UpdatedDate = DateTime.Now;
            entity.Status = false;
            _db.GuardianTasks.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

