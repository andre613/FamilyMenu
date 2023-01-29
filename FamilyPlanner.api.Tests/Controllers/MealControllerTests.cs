using AutoFixture;
using FamilyPlanner.api.Controllers;
using FamilyPlanner.api.Entities;
using FamilyPlanner.api.Repositories;
using Moq;

namespace FamilyPlanner.api.Tests.Controllers
{
    [TestClass]
    public class MealControllerTests : TestBase
    {
        private readonly Mock<IRepository<Meal>> _mockRepository;

        public MealControllerTests()
        {
            _mockRepository = new Mock<IRepository<Meal>>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockRepository.Reset();
        }

        [TestMethod]
        public void ConstructorReturnsMealController()
        {
            var mealController = new MealController(_mockRepository.Object);
            
            Assert.IsNotNull(mealController);
            Assert.IsInstanceOfType<MealController>(mealController);
        }

        [TestMethod]
        public void PostReturnsMealObject()
        {
            _mockRepository.Setup(mr => mr.Add(It.IsAny<Meal>())).Returns(_fixture.Create<Meal>());
            var mealController = new MealController(_mockRepository.Object);
            
            var newMeal = _fixture.Build<Meal>()
                .Without(m => m.Id)
                .Create();

            var actual = mealController.Post(newMeal);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<Meal>(actual);
        }

        [TestMethod]
        public void PostProperlySavesToRepository()
        {

            _mockRepository.Setup(mr => mr.Add(It.IsAny<Meal>())).Returns(_fixture.Create<Meal>());
            var mealController = new MealController(_mockRepository.Object);

            var newMeal = _fixture.Build<Meal>()
                .Without(m => m.Id)
                .Create();

            var actual = mealController.Post(newMeal);

            _mockRepository.Verify(mr => mr.Add(It.IsAny<Meal>()), Times.Once);
        }
    }
}