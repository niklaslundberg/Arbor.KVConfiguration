using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Urns.Urn))]
    public class when_having_two_equal_urns_with_different_casing
    {
        protected static Urns.Urn urn1;

        protected static Urns.Urn urn2;

        Establish context = () =>
        {
            urn1 = new Urns.Urn("urn:abc");
            urn2 = new Urns.Urn("URN:ABC");
        };

        Because of = () => { };

        Behaves_like<two_equal_urns> two_equal_urns;
    }
}
