using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Urns.Urn))]
    public class when_checking_is_in_hierarchy_for_null_rn
    {
        private static string attempted_value = "urn:a:b:c";

        protected static Urns.Urn urn;
        
        private Establish context = () => {urn= new Urns.Urn(attempted_value); };

        private Because of = () => { result = urn.IsInHierarchy(null); };

        private It should_false = () => result.ShouldBeFalse();

        private static bool result;
    }
}