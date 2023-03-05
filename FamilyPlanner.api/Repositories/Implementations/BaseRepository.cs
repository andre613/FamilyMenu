using FamilyPlanner.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyPlanner.api.Repositories.Implementations
{
    public class EntityNotFoundException<T> : Exception where T : BaseEntity
    {
        public EntityNotFoundException(string message) : base(message) { }
    }

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

        public void Update(T entity)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var entityToUpdate = dbContext.Set<T>().SingleOrDefault(x => x.Id == entity.Id) ?? 
                throw new EntityNotFoundException<T>($"Unable to perform update.\nEntity of type {typeof(T).Name} with ID {entity.Id} not found.");
            dbContext.UpdateEntity(entityToUpdate, entity);
        }
    }
}