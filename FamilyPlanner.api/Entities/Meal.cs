namespace FamilyPlanner.api.Entities
{
    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack,
        Other
    }

    public class Meal : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string RecipeUri { get; set;} = null!;

        public virtual ICollection<MealType> MealTypes { get; set; } = new List<MealType>();

        public virtual ICollection<GroceryListItem> GroceryListItems { get; set; } = new List<GroceryListItem>();
    }
}