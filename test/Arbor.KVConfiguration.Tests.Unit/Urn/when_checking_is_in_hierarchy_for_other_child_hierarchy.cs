using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Primitives.Urn))]
    public class when_checking_is_in_hierarchy_for_other_child_hierarchy
    {
        private static readonly string attempted_value = "urn:a:b:c";

        protected static Primitives.Urn urn;

        private static bool result;

        private Establish context = () => { urn = Primitives.Urn.Parse(attempted_value); };

        private Because of = () => { result = urn.IsInHierarchy(Primitives.Urn.Parse("urn:a:b:c:d")); };

        private It should_be_false = () => result.ShouldBeFalse();
    }
}