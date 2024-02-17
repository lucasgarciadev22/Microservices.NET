using Microsoft.EntityFrameworkCore;
using Movies.Core.Entities.Base;
using Movies.Core.Repositories.Base;
using Movies.Infrastructure.Data;

namespace Movies.Infrastructure.Repositores.Base;

public class Repository<T>(MovieContext movieContext) : IRepository<T>
    where T : Entity
{
    protected readonly MovieContext _movieContext = movieContext;

    public async Task<IReadOnlyList<T>> GetAllAsync() => await _movieContext.Set<T>().ToListAsync();

    public async Task<T> GetByIdAsync(int id) => await _movieContext.Set<T>().FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        await _movieContext.Set<T>().AddAsync(entity);

        await _movieContext.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _movieContext.Entry(entity).State = EntityState.Modified;

        int updatedCount = await _movieContext.SaveChangesAsync();

        return updatedCount > 0;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        _movieContext.Set<T>().Remove(entity);

        int deletedCount = await _movieContext.SaveChangesAsync();

        return deletedCount > 0;
    }
}
