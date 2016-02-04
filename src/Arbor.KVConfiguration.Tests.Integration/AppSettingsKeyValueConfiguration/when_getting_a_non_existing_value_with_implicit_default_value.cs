using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.AppSettingsKeyValueConfiguration
{
    public class when_getting_a_non_existing_value_with_implicit_default_value
    {
        private static Core.IKeyValueConfiguration configuration;

        private static string value;

        private Establish context = () => {
            configuration = new SystemConfiguration.AppSettingsKeyValueConfiguration();
        };

        private Because of = () => { value = configuration.ValueOrDefault("d"); };

        private It return_existing_value = () => { value.ShouldEqual(""); };
    }
}
