using Guardian.Domain.Models;
using System.Linq.Expressions;

namespace Guardian.Application.Interfaces.IRepository
{
    public  interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsync(T modal);
        Task RemoveAsync(T modal);
        Task SaveAsync();
    }
}
