using Microsoft.EntityFrameworkCore;
using Store.Context;

namespace Store.Repositories
{
  public class GenericRepository<TEntity>(AppDbContext _dbContext) where TEntity : class
  {
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
      return await _dbContext.Set<TEntity>().ToListAsync();
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
