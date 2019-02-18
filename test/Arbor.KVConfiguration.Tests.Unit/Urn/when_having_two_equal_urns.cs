using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
#pragma warning disable 0649
#pragma warning disable 0169
    [Subject(typeof(Arbor.Primitives.Urn))]
    public class when_having_two_equal_urns
    {
        protected static Arbor.Primitives.Urn urn1;

        protected static Arbor.Primitives.Urn urn2;

        private Establish context = () =>
        {
            urn1 = new Arbor.Primitives.Urn("urn:abc");
            urn2 = new Arbor.Primitives.Urn("urn:abc");
        };

        private Because of = () => { };

        private Behaves_like<two_equal_urns> two_equal_urns;
    }
}
