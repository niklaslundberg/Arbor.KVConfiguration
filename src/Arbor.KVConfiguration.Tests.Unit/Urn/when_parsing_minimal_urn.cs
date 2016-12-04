using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{

#pragma warning disable 0649
#pragma warning disable 0169
    [Subject(typeof(Urns.Urn))]
    public class when_parsing_minimal_urn
    {
        static string attempted_value;

        protected static Urns.Urn urn;

        Behaves_like<a_one_level_urn> a_one_level_urn;

        Establish context = () => { attempted_value = "urn:abc"; };

        Because of = () => { Urns.Urn.TryParse(attempted_value, out urn); };

        It should_not_be_null = () => urn.ShouldNotBeNull();
    }
}
