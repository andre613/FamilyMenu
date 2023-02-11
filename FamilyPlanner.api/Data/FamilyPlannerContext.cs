using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace FamilyPlanner.Common.Entities
{
    public class EnumCollectionJsonValueConverter<T> : ValueConverter<ICollection<T>, string> where T : Enum
    {
        public EnumCollectionJsonValueConverter() : base(
          v => JsonConvert
            .SerializeObject(v.Select(e => e.ToString()).ToList()),
          v => JsonConvert
            .DeserializeObject<ICollection<string>>(v)!
            .Select(e => (T)Enum.Parse(typeof(T), e)).ToList())
        {}
    }

    public class CollectionValueComparer<T> : ValueComparer<ICollection<T>>
    {
        public CollectionValueComparer() : base((c1, c2) => c1!.SequenceEqual(c2!),
          c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())), c => (ICollection<T>)c.ToHashSet())
        {}
    }

    public class FamilyPlannerDataContext : DbContext
    {
        public virtual DbSet<Meal> Meals { get; set; }

        public FamilyPlannerDataContext(DbContextOptions options): base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroceryItem>()
                .Property(gi => gi.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<GroceryItem>()
                .HasKey(gi => gi.Id);

            modelBuilder.Entity<GroceryListItem>()
                .Property(gli => gli.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<GroceryListItem>()
                .HasKey(gli => gli.Id);

            modelBuilder.Entity<GroceryListItem>()
                .HasOne(gli => gli.GroceryItem)
                .WithMany()
                .HasForeignKey(gli => gli.GroceryItemId);

            modelBuilder.Entity<Meal>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();
                
            modelBuilder.Entity<Meal>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Meal>()
                .HasMany(m => m.GroceryListItems)
                .WithOne()
                .HasForeignKey(gli => gli.MealId);
        }
    }
}