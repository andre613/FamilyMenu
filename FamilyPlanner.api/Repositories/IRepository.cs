using FamilyPlanner.Common.Entities;

namespace FamilyPlanner.api.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        ICollection<T> GetAll();

        T Add(T entity);

        void Update(T entity);
    }
}