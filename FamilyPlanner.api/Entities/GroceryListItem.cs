using System.ComponentModel.DataAnnotations;

namespace FamilyPlanner.api.Entities
{
    public class GroceryListItem : BaseEntity
    {
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public uint Quantity { get; set; }

        [Required]
        public uint GroceryItemId { get; set; }

        public uint? MealId {get;set;}

        public virtual GroceryItem GroceryItem { get; set; } = null!;
    }
}