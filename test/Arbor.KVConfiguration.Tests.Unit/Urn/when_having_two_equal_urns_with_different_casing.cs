using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
#pragma warning disable 0649
#pragma warning disable 0169
    [Subject(typeof(Primitives.Urn))]
    public class when_having_two_equal_urns_with_different_casing
    {
        protected static Primitives.Urn urn1;

        protected static Primitives.Urn urn2;

        private Establish context = () =>
        {
            urn1 = Primitives.Urn.Parse("urn:abc:123");
            urn2 = Primitives.Urn.Parse("URN:ABC:123");
        };

        private Because of = () => { };

        private Behaves_like<two_equal_urns> two_equal_urns;
    }
}