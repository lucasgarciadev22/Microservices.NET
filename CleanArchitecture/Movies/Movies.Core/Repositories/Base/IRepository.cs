using Movies.Core.Entities.Base;

namespace Movies.Core.Repositories.Base;

public interface IRepository<T>
    where T : Entity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
}
