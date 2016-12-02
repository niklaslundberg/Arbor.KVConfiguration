using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(Urns.Urn))]
    public class when_parsing_two_level_urn
    {
        private static string attempted_value;

        protected static Urns.Urn urn;

        Behaves_like<a_multi_level_urn> a_multi_level_urn;

        Establish context = () => { attempted_value = "urn:abc:123"; };

        Because of = () => { Urns.Urn.TryParse(attempted_value, out urn); };

        It should_not_be_null = () => urn.ShouldNotBeNull();
    }
}