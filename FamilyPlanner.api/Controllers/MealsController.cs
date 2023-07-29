using FamilyPlanner.Common.Entities;
using FamilyPlanner.api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using FamilyPlanner.api.Repositories.Implementations;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Meal>> GetMeals() 
        {
            var meals = _mealRepository.GetAll().ToList();
            return Ok(meals);
        }

        [HttpPost(Name = "PostMeal")]
        public ActionResult<Meal> PostMeal(Meal meal) 
        {
            var newMeal = _mealRepository.Add(meal);
            return Created($"Meals/{newMeal.Id}", newMeal);
        }

        [HttpPut(Name = "PutMeal")]
        public ActionResult<Meal> PutMeal(Meal meal)
        {
            try
            {
                return  Accepted($"Meals/{meal.Id}", _mealRepository.Update(meal));
            }
            catch(EntityNotFoundException<Meal> ex)
            {
                return NotFound(new { meal.Id, error = ex.Message});
            }
        }
    }
}