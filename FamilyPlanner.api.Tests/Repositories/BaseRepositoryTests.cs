using AutoFixture;
using FamilyPlanner.api.Repositories.Implementations;
using FamilyPlanner.api.Tests.Helpers;
using FamilyPlanner.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FamilyPlanner.api.Tests.Repositories
{
    [TestClass]
    public class BaseRepositoryTests : TestBase
    {
        private Mock<FamilyPlannerDataContext> _mockDbContext = null!;
        private Mock<IDbContextFactory<FamilyPlannerDataContext>> _mockDbContextFactory = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDbContext = CreateMockFamilyPlannerDataContext();
            _mockDbContextFactory = CreateMockFamilyPlannerDataContextFactory(_mockDbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockDbContext.Reset();
            _mockDbContextFactory.Reset();
        }

        [TestMethod]
        public void ConstructorReturnsBaseRepository()
        {
            var repository = new BaseRepository<Meal>(_mockDbContextFactory.Object);

            Assert.IsNotNull(repository);
            Assert.IsInstanceOfType<BaseRepository<Meal>>(repository);
        }

        [TestMethod]
        public void GetAllReturnsListOfCorrectType()
        {
            var mockDbContext = CreateMockFamilyPlannerDataContext();

            mockDbContext
                .Setup(mdc => mdc.Set<Meal>())
                .Returns(CreateMockDbSet(new List<Meal>()).Object)
                .Verifiable();

            _mockDbContextFactory
                .Setup(mdcf => mdcf.CreateDbContext())
                .Returns(mockDbContext.Object)
                .Verifiable();

            var repository = new BaseRepository<Meal>(_mockDbContextFactory.Object);
            var actual = repository.GetAll();

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<List<Meal>>(actual);
        }

        [TestMethod]
        public void GetAllReturnsCorrectData()
        {
            var data = _fixture
                .CreateMany<Meal>()
                .ToList();

            var mockDbSet = CreateMockDbSet(data);
            var mockDbContext = CreateMockFamilyPlannerDataContext();

            mockDbContext
                .Setup(mdc => mdc.Set<Meal>())
                .Returns(mockDbSet.Object)
                .Verifiable();

            _mockDbContextFactory
                .Setup(mdcf => mdcf.CreateDbContext())
                .Returns(mockDbContext.Object)
                .Verifiable();

            var repository = new BaseRepository<Meal>(_mockDbContextFactory.Object);
            var actual = repository.GetAll();

            ObjectsComparerHelper.AssertAreEqual(data, actual);
        }

        [TestMethod]
        public void AddReturnsCorrectType()
        {
            var repository = new BaseRepository<Meal>(_mockDbContextFactory.Object);
            var newMeal = _fixture.Create<Meal>();

            var actual = repository.Add(newMeal);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<Meal>(actual);
        }

        [TestMethod]
        public void AddProperlySavesToDB()
        {
            _mockDbContext.Setup(mdc => 
                mdc.Add(It.IsAny<Meal>()))
                .Callback<Meal>(m => 
                    m.Id = _fixture.Create<uint>());

            var repository = new BaseRepository<Meal>(_mockDbContextFactory.Object);
            
            var newMeal = _fixture.Build<Meal>()
                .Without(m => m.Id)
                .Create();

            var actual = repository.Add(newMeal);

            _mockDbContext.Verify(mds => mds.Add(It.IsAny<Meal>()), Times.Once());
            _mockDbContext.Verify(mdc => mdc.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void UpdateProperlySavesToDB()
        {
            var data =
                _fixture
                    .CreateMany<Meal>()
                    .ToList();

            data.Add(
                _fixture
                    .Build<Meal>()
                    .With(m => m.Id, 123u)
                    .Create());

            _mockDbContext
                .Setup(mdc => mdc.Set<Meal>())
                .Returns(CreateMockDbSet(data).Object);

            var repository = new BaseRepository<Meal>(_mockDbContextFactory.Object);

            var updatedMeal = new Meal
            {
                Id = 123u,
                Description = "Updated Description",
                MealType = MealType.Snack,
                Name = "Updated Name",
                RecipeUri = "http://google.ca"
            };

            repository.Update(updatedMeal);

            _mockDbContext.Verify(mdc => mdc.Set<Meal>(), Times.Once);
            _mockDbContext.Verify(mdc => mdc.UpdateEntity(It.IsAny<Meal>(), It.IsAny<Meal>()), Times.Once);
        }

        [TestMethod]
        public void UpdateThrowsCorrectExceptionIfIdNotExist()
        {
            var data =
                _fixture
                    .Build<Meal>()
                    .With(m => m.Id, _fixture.Create<uint>() + 123u)
                    .CreateMany()
                    .ToList();

            _mockDbContext
                .Setup(mdc => mdc.Set<Meal>())
                .Returns(CreateMockDbSet(data).Object);

            var repository = new BaseRepository<Meal>(_mockDbContextFactory.Object);

            var updatedMeal = new Meal
            {
                Id = 123u,
                Description = "Updated Description",
                MealType = MealType.Snack,
                Name = "Updated Name",
                RecipeUri = "http://google.ca"
            };

            Assert.ThrowsException<EntityNotFoundException<Meal>>(() => repository.Update(updatedMeal));
        }

        private static Mock<DbSet<Meal>> CreateMockDbSet(List<Meal> data)
        {
            var dataAsQueryable = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<Meal>>();

            var mockQueryable = mockDbSet.As<IQueryable<Meal>>();
            mockQueryable.Setup(mq => mq.Provider).Returns(dataAsQueryable.Provider);
            mockQueryable.Setup(mq => mq.Expression).Returns(dataAsQueryable.Expression);
            mockQueryable.Setup(mq => mq.ElementType).Returns(dataAsQueryable.ElementType);
            mockQueryable.Setup(mq => mq.GetEnumerator()).Returns(dataAsQueryable.GetEnumerator());

            return mockDbSet;
        }

        private static Mock<FamilyPlannerDataContext> CreateMockFamilyPlannerDataContext()
        {
            var mockFamilyPlannerDataContext = new Mock<FamilyPlannerDataContext>(
                MockBehavior.Default,
                new object[]
                {
                    new DbContextOptions<FamilyPlannerDataContext>()
                }) ;

            return mockFamilyPlannerDataContext;
        }

        private static Mock<IDbContextFactory<FamilyPlannerDataContext>> CreateMockFamilyPlannerDataContextFactory(Mock<FamilyPlannerDataContext>? mockFamilyPlannerDataContext = null)
        {
            var mockFamilyPlannerDataContextFactory = new Mock<IDbContextFactory<FamilyPlannerDataContext>>();
            mockFamilyPlannerDataContextFactory
                .Setup(madcf => madcf.CreateDbContext())
                .Returns((mockFamilyPlannerDataContext ?? CreateMockFamilyPlannerDataContext()).Object);

            return mockFamilyPlannerDataContextFactory;
        }
    }
}