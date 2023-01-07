using Guardian.Application.Interfaces.IRepository;
using Guardian.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Guardian.Data.Repository
{
    public class GuardianRepository : IGuardianRepository
    {
        private readonly ApplicationDbContext _db;
        public GuardianRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(GuardianModel entity)
        {
            await _db.Guardians.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<GuardianModel> GetAsync(Expression<Func<GuardianModel, bool>> filter = null, bool tracked = true)
        {
            //It does not get execute right away
            IQueryable<GuardianModel> query = _db.Guardians;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<GuardianModel>> GetAllAsync(Expression<Func<GuardianModel, bool >> filter = null)
        {
            //It does not get execute right away
            IQueryable<GuardianModel> query = _db.Guardians;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync(); //This is deffered execution. ToList() causes immediate execution
        }

        public async Task RemoveAsync(GuardianModel entity)
        {
            _db.Guardians.Remove(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(GuardianModel entity)
        {
            _db.Guardians.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
