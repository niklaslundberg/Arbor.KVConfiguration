using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Primitives.Urn))]
    public class when_checking_is_in_hierarchy_for_null_rn
    {
        private static readonly string attempted_value = "urn:a:b:c";

        protected static Primitives.Urn urn;

        private static bool result;

        private Establish context = () => urn = Primitives.Urn.Parse(attempted_value);

        private Because of = () => result = urn.IsInHierarchy(default);

        private It should_false = () => result.ShouldBeFalse();
    }
}