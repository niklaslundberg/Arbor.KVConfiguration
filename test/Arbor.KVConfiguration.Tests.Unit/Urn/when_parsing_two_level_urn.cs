using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
#pragma warning disable 0649
#pragma warning disable 0169
    [Subject(typeof(Primitives.Urn))]
    public class when_parsing_two_level_urn
    {
        private static string attempted_value;

        protected static bool has_value;

        protected static Primitives.Urn urn;

        private Behaves_like<a_multi_level_urn> a_multi_level_urn;

        private Establish context = () => { attempted_value = "urn:test:abc:123"; };

        private Because of = () =>
        {
            parsed = Primitives.Urn.TryParse(attempted_value, out var value);
            has_value = value.HasValue;
            urn = value.Value;
        };

        private It should_not_be_null = () => has_value.ShouldBeTrue();
        private It should_be_parsed = () => parsed.ShouldBeTrue();
        private static bool parsed;
    }
}