using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
#pragma warning disable 0649
#pragma warning disable 0169
    [Subject(typeof(Primitives.Urn))]
    public class when_parsing_minimal_urn
    {
        private static string attempted_value;

        protected static Primitives.Urn urn;

        private Behaves_like<a_one_level_urn> a_one_level_urn;

        private Establish context = () => { attempted_value = "urn:abc:123"; };

        private Because of = () => { Primitives.Urn.TryParse(attempted_value, out urn); };

        private It should_not_be_null = () => urn.ShouldNotBeNull();
    }
}