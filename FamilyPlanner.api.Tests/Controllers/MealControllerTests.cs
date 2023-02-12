using AutoFixture;
using FamilyPlanner.api.Controllers;
using FamilyPlanner.Common.Entities;
using FamilyPlanner.api.Repositories;
using Moq;
using FamilyPlanner.api.Tests.Helpers;

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
            var mealsController = new MealsController(_mockRepository.Object);

            Assert.IsNotNull(mealsController);
            Assert.IsInstanceOfType<MealsController>(mealsController);
        }

        [TestMethod]
        public void GetMealsReturnsCorrectType()
        {
            _mockRepository.Setup(mr => mr.GetAll()).Returns(new List<Meal>());
            var mealsController = new MealsController(_mockRepository.Object);

            var actual = mealsController.GetMeals();

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<List<Meal>>(actual);
        }

        [TestMethod]
        public void GetMealsReturnsCorrectData()
        {
            var data = _fixture.CreateMany<Meal>().ToList();
            _mockRepository.Setup(mr => mr.GetAll()).Returns(data);
            var mealsController = new MealsController(_mockRepository.Object);

            var actual = mealsController.GetMeals();

            ObjectsComparerHelper.AssertAreEqual(data, actual);
        }

        [TestMethod]
        public void PostReturnsMealObject()
        {
            _mockRepository.Setup(mr => mr.Add(It.IsAny<Meal>())).Returns(_fixture.Create<Meal>());
            var mealsController = new MealsController(_mockRepository.Object);
            
            var newMeal = _fixture.Build<Meal>()
                .Without(m => m.Id)
                .Create();

            var actual = mealsController.Post(newMeal);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<Meal>(actual);
        }

        [TestMethod]
        public void PostProperlySavesToRepository()
        {

            _mockRepository.Setup(mr => mr.Add(It.IsAny<Meal>())).Returns(_fixture.Create<Meal>());
            var mealsController = new MealsController(_mockRepository.Object);

            var newMeal = _fixture.Build<Meal>()
                .Without(m => m.Id)
                .Create();

            var actual = mealsController.Post(newMeal);

            _mockRepository.Verify(mr => mr.Add(It.IsAny<Meal>()), Times.Once);
        }
    }
}