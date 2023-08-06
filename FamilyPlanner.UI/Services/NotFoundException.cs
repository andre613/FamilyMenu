using FamilyPlanner.Common.Entities;

namespace FamilyPlanner.UI.Services
{
    public class NotFoundException<T> : Exception where T : BaseEntity
    {
        public NotFoundException(uint id) : 
            base($"{typeof(T).Name} with ID {id} not Found.)"){}
    }
}