using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    public class when_parsing_too_short_urn
    {
        private static string attempted_value;

        protected static Primitives.Urn? urn;

        private static bool parsed;

        private Establish context = () => { attempted_value = "urn:logLevel"; };

        private Because of = () => { parsed = Primitives.Urn.TryParse(attempted_value, out urn); };

        private It should_be_null = () => urn.ShouldBeNull();

        private It should_not_parse_successfully = () => parsed.ShouldBeFalse();
    }
}