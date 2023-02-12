using ObjectsComparer;

namespace FamilyPlanner.api.Tests.Helpers
{
    internal static class ObjectsComparerHelper
    {
        public static void AssertAreEqual<T>(T expected, T actual)
        {
            var comparer = new ObjectsComparer.Comparer<T>();
            var areEqual = comparer.Compare(expected, actual, out IEnumerable<Difference> differences);
            Assert.IsTrue(areEqual, $"Differences found between expected and actual: {string.Join(Environment.NewLine, differences)}");
        }
    }
}