using FamilyPlanner.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyPlanner.api.Repositories.Implementations
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IDbContextFactory<FamilyPlannerDataContext> _dbContextFactory;

        public BaseRepository(IDbContextFactory<FamilyPlannerDataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public T Add(T entity)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                dbContext.Add(entity);
                dbContext.SaveChanges();
            }

            return entity;
        }

        public ICollection<T> GetAll()
        {
            DbSet<T> dbSet;

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                dbSet = dbContext.Set<T>();
            }

            return dbSet.ToList();
        }
    }
}