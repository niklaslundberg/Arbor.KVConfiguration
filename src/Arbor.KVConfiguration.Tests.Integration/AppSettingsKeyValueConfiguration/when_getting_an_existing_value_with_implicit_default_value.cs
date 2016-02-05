using Arbor.KVConfiguration.Core;

using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.AppSettingsKeyValueConfiguration
{
    public class when_getting_an_existing_value_with_implicit_default_value
    {
        private static Core.IKeyValueConfiguration configuration;

        private static string value;

        private Establish context = () =>
        {
            configuration = new SystemConfiguration.AppSettingsKeyValueConfiguration();
        };

        private Because of = () => { value = configuration.ValueOrDefault("a"); };

        private It return_existing_value = () => { value.ShouldEqual("b"); };
    }
}
