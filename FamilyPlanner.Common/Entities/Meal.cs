using System.ComponentModel.DataAnnotations;

namespace FamilyPlanner.Common.Entities
{
    [Flags]
    public enum MealType
    {
        Other = 1,
        Breakfast = 2,
        Lunch = 4,
        Dinner = 8,
        Snack = 16
    }

    public class Meal : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Url]
        public string? RecipeUri { get; set;} = null;

        public MealType? MealType { get; set; } = null;

        public virtual IList<GroceryListItem> GroceryListItems { get; set; } = new List<GroceryListItem>();
    }
}