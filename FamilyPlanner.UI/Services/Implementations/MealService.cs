using FamilyPlanner.Common.Entities;
using System.Net;

namespace FamilyPlanner.UI.Services.Implementations
{
    public class NotFoundException : Exception
    {

    }

    public class MealService : IMealService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MealService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Meal> AddMeal(Meal meal)
        {
            using var httpClient = CreateFamilyPlannerClient();
            var response = await httpClient.PostAsJsonAsync("meals", meal);
            return (await response.Content.ReadFromJsonAsync<Meal>())!;
        }

        public async Task<Meal?> GetMeal(uint mealId)
        {
            using var httpClient = CreateFamilyPlannerClient();
            var response = await httpClient.GetAsync($"meals/{mealId}");

            if(!response.IsSuccessStatusCode)
            {
                ParseResponse(response, mealId);
            }

            return await response.Content.ReadFromJsonAsync<Meal>();
        }

        public async Task<List<Meal>> GetMeals()
        {
            using var httpClient = CreateFamilyPlannerClient();
            var response = await httpClient.GetAsync("meals");

            if(!response.IsSuccessStatusCode)
            {
                ParseResponse(response);
            }

            return (await response.Content.ReadFromJsonAsync<List<Meal>>())!;
        }

        public async Task<Meal> UpdateMeal(Meal meal)
        {
            using var httpClient = CreateFamilyPlannerClient();
            var response = await httpClient.PutAsJsonAsync("meals", meal);

            if(!response.IsSuccessStatusCode)
            {
                ParseResponse(response, meal.Id);
            }

            return (await response.Content.ReadFromJsonAsync<Meal>())!;
        }

        private static void ParseResponse(HttpResponseMessage response, uint? mealId = null)
        {
            throw response.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundException<Meal>(mealId!.Value),
                _ => new Exception("Internal Error. Please contact administrator."),
            };
        }

        private HttpClient CreateFamilyPlannerClient()
        {
            var client = _httpClientFactory.CreateClient("FamilyPlanner.Api");

            if (client.BaseAddress == null)
            {
                throw new NullReferenceException(nameof(client.BaseAddress));
            }

            return client;
        }
    }
}