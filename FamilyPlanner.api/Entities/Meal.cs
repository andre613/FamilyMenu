using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public string? RecipeUri { get; set;} = null;

        public virtual ICollection<MealType> MealTypes { get; set; } = new List<MealType>();

        public virtual ICollection<GroceryListItem> GroceryListItems { get; set; } = new List<GroceryListItem>();
    }
}