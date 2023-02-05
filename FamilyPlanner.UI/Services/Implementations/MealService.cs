using FamilyPlanner.Common.Entities;

namespace FamilyPlanner.UI.Services.Implementations
{
    public class MealService : IMealService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MealService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Meal?> AddMeal(Meal meal)
        {
            using (var httpClient = _httpClientFactory.CreateClient("FamilyPlanner.API"))
            {
                if(httpClient.BaseAddress == null)
                {
                    throw new NullReferenceException(nameof(httpClient.BaseAddress));
                }

                var response = await httpClient.PostAsJsonAsync("meals", meal);
                return await response.Content.ReadFromJsonAsync<Meal>();
            }
        }
    }
}