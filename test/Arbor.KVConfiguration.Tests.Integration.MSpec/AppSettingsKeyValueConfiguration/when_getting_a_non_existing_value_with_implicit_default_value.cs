using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Extensions.StringExtensions;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.MSpec.AppSettingsKeyValueConfiguration
{
    [Subject(typeof(SystemConfiguration.AppSettingsKeyValueConfiguration))]
    public class when_getting_a_non_existing_value_with_implicit_default_value
    {
        static IKeyValueConfiguration configuration;

        static string value;

        Establish context = () =>
        {
            configuration = new SystemConfiguration.AppSettingsKeyValueConfiguration();
        };

        Because of = () => { value = configuration.ValueOrDefault("d"); };

        It return_existing_value = () => { value.ShouldEqual(""); };
    }
}