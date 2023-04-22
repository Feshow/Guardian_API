using System.Linq.Expressions;

namespace Guardian.Domain.Interfaces.IRepository
{
    public  interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null); //Last parameter indicates when I need to retrive the Guardian object based on FK Table relatio (EF)
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(T modal);
        Task RemoveAsync(T modal);
        Task SaveAsync();
    }
}
