using FamilyPlanner.Common.Entities;
using FamilyPlanner.api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FamilyPlanner.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MealsController : ControllerBase
    {
        private readonly IRepository<Meal> _mealRepository;

        public MealsController(IRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        [HttpGet(Name ="GetMeals")]
        public List<Meal> GetMeals() 
        {
            return _mealRepository.GetAll().ToList();
        }

        [HttpPost(Name = "PostMeal")]
        public Meal Post(Meal meal) 
        {
            return _mealRepository.Add(meal);
        }

        [HttpPut(Name = "PutMeal")]
        public void Put(Meal meal)
        {
            _mealRepository.Update(meal);
        }
    }
}