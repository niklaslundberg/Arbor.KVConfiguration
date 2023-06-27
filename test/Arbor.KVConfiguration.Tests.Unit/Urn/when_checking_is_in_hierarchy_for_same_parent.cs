using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Primitives.Urn))]
    public class when_checking_is_in_hierarchy_for_same_parent
    {
        private static readonly string attempted_value = "urn:test:a:b:c";

        protected static Primitives.Urn urn;

        private static bool result;

        private Establish context = () => urn = Primitives.Urn.Parse(attempted_value);

        private Because of = () => result = urn.IsInHierarchy(Primitives.Urn.Parse("urn:test:a"));

        private It should_be_true = () => result.ShouldBeTrue();
    }
}