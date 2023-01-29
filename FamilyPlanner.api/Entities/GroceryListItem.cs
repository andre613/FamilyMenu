namespace FamilyPlanner.api.Entities
{
    public class GroceryListItem : BaseEntity
    {
        public string Description { get; set; } = null!;

        public uint Quantity { get; set; }

        public uint GroceryItemId { get; set; }

        public uint? MealId {get;set;}

        public virtual GroceryItem GroceryItem { get; set; } = null!;

        public virtual Meal? Meal { get; set; } = null!;
    }
}
