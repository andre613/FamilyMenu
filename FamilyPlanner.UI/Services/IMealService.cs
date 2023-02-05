using FamilyPlanner.Common.Entities;

namespace FamilyPlanner.UI.Services
{
    public interface IMealService
    {
        Task<Meal?> AddMeal(Meal meal);
    }
}
