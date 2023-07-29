using AutoFixture;

namespace FamilyPlanner.api.Tests
{
    public abstract class TestBase
    {
        protected readonly Fixture _fixture;

        protected TestBase()
        {
            _fixture = new Fixture();

            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}