using System.ComponentModel.DataAnnotations;

namespace FamilyPlanner.api.Entities
{
    public enum GroceryDepartment
    {
        Bakery,
        Bulk,
        Dairy,
        Deli,
        Frozen,        
        Meat,
        Pantry,
        Produce
    }

    public class GroceryItem : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        public GroceryDepartment? GroceryDepartment { get; set; }
    }
}