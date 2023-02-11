using AutoFixture;
using FamilyPlanner.Common.Entities;
using FamilyPlanner.UI.Services.Implementations;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace FamilyPlanner.UI.Tests.Services
{
    [TestClass]
    public class MealServiceTests : TestBase
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory = new();

        [TestCleanup]
        public void Cleanup()
        {
            _mockHttpClientFactory.Reset();
        }

        [TestMethod]
        public void ConstructorReturnsCorrectObject()
        {
            var mealService = new MealService(_mockHttpClientFactory.Object);

            Assert.IsNotNull(mealService);
            Assert.IsInstanceOfType<MealService>(mealService);
        }

        [TestMethod]
        public void AddMealReturnsTaskOfTypeMeal()
        {
            var mealService = new MealService(_mockHttpClientFactory.Object);
            _mockHttpClientFactory
                .Setup(mhcf => mhcf.CreateClient("FamilyPlanner.API"))
                .Returns(new HttpClient(new Mock<HttpMessageHandler>().Object)
                {
                    BaseAddress = new Uri("https://www.test.com")
                });

            var newMeal = _fixture
                .Build<Meal>()
                .Without(m => m.Id)
                .Create();

            var actual = mealService.AddMeal(newMeal);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType<Task<Meal>>(actual);
        }

        [TestMethod]
        public async Task AddMealThrowsExceptionIfApiUriIsNotConfigured()
        {
            var httpClient = new HttpClient(new Mock<HttpMessageHandler>().Object);
            _mockHttpClientFactory
                .Setup(mhcf => mhcf.CreateClient("FamilyPlanner.API"))
                .Returns(httpClient);

            var mealService = new MealService(_mockHttpClientFactory.Object);

            await Assert.ThrowsExceptionAsync<NullReferenceException>(
                () => mealService
                    .AddMeal(_fixture
                        .Build<Meal>()
                        .Without(m => m.Id)
                        .Create()));
        }

        [TestMethod]
        public async Task AddMealCallsApi()
        {
            var numSendAsyncCalls = 0;

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var addedMeal = JsonConvert.SerializeObject(_fixture.Create<Meal>());

            mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                )
            .Callback(() => numSendAsyncCalls++)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(addedMeal)

            }).Verifiable();

            _mockHttpClientFactory.Setup(mhcf => mhcf.CreateClient("FamilyPlanner.Api"))
                .Returns(() =>
                {
                    return new HttpClient(mockHttpMessageHandler.Object)
                    {
                        BaseAddress = new Uri("https://www.test.com")
                    };
                });

            var mealService = new MealService(_mockHttpClientFactory.Object);
            
            var actualResult = await mealService.AddMeal(_fixture
                .Build<Meal>()
                .Without(m => m.Id)
                .Create()
            );

            _mockHttpClientFactory.Verify(mhcf => mhcf.CreateClient("FamilyPlanner.Api"), Times.Once);
            mockHttpMessageHandler.Verify();
        }
    }
}