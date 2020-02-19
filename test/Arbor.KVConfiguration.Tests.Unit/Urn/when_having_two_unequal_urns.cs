using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
#pragma warning disable 0649
#pragma warning disable 0169
    [Subject(typeof(Primitives.Urn))]
    public class when_having_two_unequal_urns
    {
        protected static Primitives.Urn urn1;

        protected static Primitives.Urn urn2;

        private Establish context = () =>
        {
            urn1 = new Primitives.Urn("urn:abc:123");
            urn2 = new Primitives.Urn("urn:def:123");
        };

        private Because of = () => { };

        private Behaves_like<two_unequal_urns> two_unequal_urns;
    }
}