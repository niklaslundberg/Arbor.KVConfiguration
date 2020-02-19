namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Primitives.Urn))]
    public class when_checking_is_in_hierarchy_for_same_urn
    {
        private static readonly string attempted_value = "urn:a:b:c";

        protected static Primitives.Urn urn;

        private static bool result;

        private Establish context = () => { urn = new Primitives.Urn(attempted_value); };

        private Because of = () => { result = urn.IsInHierarchy(new Primitives.Urn(attempted_value)); };

        private It should_be_true = () => result.ShouldBeTrue();
    }
}