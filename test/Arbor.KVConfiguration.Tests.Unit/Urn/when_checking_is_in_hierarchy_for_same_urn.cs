using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Urns.Urn))]
    public class when_checking_is_in_hierarchy_for_same_urn
    {
        private static string attempted_value = "urn:a:b:c";

        protected static Urns.Urn urn;
        
        private Establish context = () => {urn= new Urns.Urn(attempted_value); };

        private Because of = () => { result = urn.IsInHierarchy(new Urns.Urn(attempted_value)); };

        private It should_be_true = () => result.ShouldBeTrue();

        private static bool result;
    }
}