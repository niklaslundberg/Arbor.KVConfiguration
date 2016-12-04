using Arbor.KVConfiguration.Core;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.AppSettingsKeyValueConfiguration
{
    [Subject(typeof(SystemConfiguration.AppSettingsKeyValueConfiguration))]
    public class when_getting_a_non_existing_value_with_explicit_default_value
    {
        static IKeyValueConfiguration configuration;

        static string value;

        Establish context =
            () => { configuration = new SystemConfiguration.AppSettingsKeyValueConfiguration(); };

        Because of = () => { value = configuration.ValueOrDefault("d", "e"); };

        It return_existing_value = () => { value.ShouldEqual("e"); };
    }
}
