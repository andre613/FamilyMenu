using FamilyPlanner.api.Repositories;
using FamilyPlanner.api.Repositories.Implementations;
using FamilyPlanner.Common.Entities;
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

        [HttpGet(Name = "GetMeals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Meal>> GetMeals() 
        {
            var meals = _mealRepository.GetAll().ToList();
            return Ok(meals);
        }

        [HttpGet("{mealId}", Name = "GetMealsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Meal> GetMealById(uint mealId)
        {
            try
            {
                return Ok(_mealRepository.GetById(mealId));
            }
            catch(EntityNotFoundException<Meal> ex)
            {
                return NotFound(new ProblemDetails 
                { 
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not found"
                });
            }
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
                return NotFound(new ProblemDetails
                {
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found"
                });
            }
        }
    }
}