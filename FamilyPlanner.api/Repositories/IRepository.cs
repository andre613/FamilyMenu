using FamilyPlanner.Common.Entities;

namespace FamilyPlanner.api.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        ICollection<T> GetAll();

        T GetById(uint Id);

        T Add(T entity);

        T Update(T entity);
    }
}