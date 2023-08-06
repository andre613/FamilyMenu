using FamilyPlanner.Common.Entities;

namespace FamilyPlanner.UI.Services
{
    public interface IMealService
    {
        Task<Meal> AddMeal(Meal meal);

        Task<Meal?> GetMeal(uint id);

        Task<List<Meal>> GetMeals();

        Task<Meal> UpdateMeal(Meal meal);
    }
}
