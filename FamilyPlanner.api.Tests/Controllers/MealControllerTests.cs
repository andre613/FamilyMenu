using AutoFixture;
using FamilyPlanner.api.Controllers;
using FamilyPlanner.api.Repositories;
using FamilyPlanner.api.Repositories.Implementations;
using FamilyPlanner.api.Tests.Helpers;
using FamilyPlanner.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var result = actual!.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
            Assert.IsInstanceOfType<List<Meal>>(result.Value);
        }

        [TestMethod]
        public void GetMealsReturnsCorrectData()
        {
            var data = _fixture.CreateMany<Meal>().ToList();
            _mockRepository.Setup(mr => mr.GetAll()).Returns(data);
            var mealsController = new MealsController(_mockRepository.Object);

            var actual = mealsController.GetMeals();
            var result = actual!.Result as OkObjectResult;

            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
            ObjectsComparerHelper.AssertAreEqual(data, result!.Value);
        }

        [TestMethod]
        public void PostReturnsMealObject()
        {
            _mockRepository.Setup(mr => mr.Add(It.IsAny<Meal>())).Returns(_fixture.Create<Meal>());
            var mealsController = new MealsController(_mockRepository.Object);
            
            var newMeal = _fixture.Build<Meal>()
                .Without(m => m.Id)
                .Create();

            var actual = mealsController.PostMeal(newMeal);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<ActionResult<Meal>>(actual);
        }

        [TestMethod]
        public void PostProperlySavesToRepository()
        {

            _mockRepository.Setup(mr => mr.Add(It.IsAny<Meal>())).Returns(_fixture.Create<Meal>());
            var mealsController = new MealsController(_mockRepository.Object);

            var newMeal = _fixture.Build<Meal>()
                .Without(m => m.Id)
                .Create();

            var actual = mealsController.PostMeal(newMeal);

            _mockRepository.Verify(mr => mr.Add(newMeal), Times.Once);
        }

        [TestMethod]
        public void PutProperlySavesToRepository()
        {
            var mealsController = new MealsController(_mockRepository.Object);
            var updatedMeal = _fixture.Create<Meal>();

            var actual = mealsController.PutMeal(updatedMeal);
            _mockRepository.Verify(mr => mr.Update(updatedMeal), Times.Once);

            var result = actual.Result as AcceptedResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status202Accepted, result!.StatusCode);
            Assert.IsTrue(result.Location!.Equals($"Meals/{updatedMeal.Id}"));
        }

        [TestMethod]
        public void PutReturnsNotFoundIfIdNotExists()
        {
            var updatedMeal = _fixture.Create<Meal>();
            
            _mockRepository
                .Setup(mr => mr.Update(updatedMeal))
                .Throws<EntityNotFoundException<Meal>>();

            var mealsController = new MealsController(_mockRepository.Object);

            var actual = mealsController.PutMeal(updatedMeal);
            var result = actual.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);
        }
    }
}