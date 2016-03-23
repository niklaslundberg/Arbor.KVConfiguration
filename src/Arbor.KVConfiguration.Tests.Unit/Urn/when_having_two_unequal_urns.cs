using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Urns.Urn))]
    public class when_having_two_unequal_urns
    {
        protected static Urns.Urn urn1;

        protected static Urns.Urn urn2;

        Establish context = () =>
            {
                urn1 = new Urns.Urn("urn:abc");
                urn2 = new Urns.Urn("urn:def");
            };

        Because of = () => { };

        Behaves_like<two_unequal_urns> two_unequal_urns;
    }
}