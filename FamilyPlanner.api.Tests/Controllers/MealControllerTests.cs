using FamilyPlanner.api.Controllers;

namespace FamilyPlanner.api.Tests.Controllers
{
    [TestClass]
    public class MealControllerTests
    {
        [TestMethod]
        public void ConstructorReturnsMealController()
        {
            var mealController = new MealController();
            
            Assert.IsNotNull(mealController);
            Assert.IsInstanceOfType<MealController>(mealController);
        }
    }
}