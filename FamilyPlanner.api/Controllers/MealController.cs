using FamilyPlanner.api.Entities;
using FamilyPlanner.api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FamilyPlanner.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MealController : ControllerBase
    {
        private readonly IRepository<Meal> _mealRepository;

        public MealController(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        [HttpPost(Name = "PostMeal")]
        public Meal Post(Meal meal) 
        {
            return _mealRepository.Add(meal);
        }
    }
}
