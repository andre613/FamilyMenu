namespace FamilyPlanner.UI.Helpers
{
    public class CheckableEnum<T> where T : Enum
    {
        public T EnumValue { get; set; } = default!;

        public bool IsChecked { get; set; } = false;
    }
}
