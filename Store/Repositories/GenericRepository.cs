using Microsoft.EntityFrameworkCore;
using Store.Context;
using System.Linq.Expressions;

namespace Store.Repositories
{
  public class GenericRepository<TEntity>(AppDbContext _dbContext) where TEntity : class
  {
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
      return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
    {
      IQueryable<TEntity> query = _dbContext.Set<TEntity>();
      foreach (var include in includes)
      {
        query = query.Include(include);
      }
      return await query.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
      await _dbContext.Set<TEntity>().AddAsync(entity);
      await _dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int entityId)
    {
      return await _dbContext.Set<TEntity>().FindAsync(entityId);
    }

    public async Task UpdateAsync(TEntity entity)
    {
      _dbContext.Set<TEntity>().Update(entity);
      await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
      _dbContext.Set<TEntity>().Remove(entity);
      await _dbContext.SaveChangesAsync();
    }
  }
}
