using Microsoft.EntityFrameworkCore;

namespace Wp.Data
{
    public interface IDbContext
  {
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    int SaveChanges();
   
    void Dispose();
  }
}
