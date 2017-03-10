using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
#pragma warning disable 0649
#pragma warning disable 0169
    [Subject(typeof(Urns.Urn))]
    public class when_having_two_equal_urns
    {
        protected static Urns.Urn urn1;

        protected static Urns.Urn urn2;

        Establish context = () =>
        {
            urn1 = new Urns.Urn("urn:abc");
            urn2 = new Urns.Urn("urn:abc");
        };

        Because of = () => { };

        Behaves_like<two_equal_urns> two_equal_urns;
    }
}
