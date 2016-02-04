using System.Collections.Specialized;

using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.InMemoryKeyValueConfiguration
{
    public class when_getting_an_existing_value_with_implicit_default_value
    {
        private static Core.InMemoryKeyValueConfiguration configuration;

        private static string value;

        private Establish context = () =>
            {
                configuration = new Core.InMemoryKeyValueConfiguration(
                    new NameValueCollection()
                        {
                            { "a", "b" }
                        });
            };

        private Because of = () => { value = configuration.ValueOrDefault("a"); };

        private It return_existing_value = () => { value.ShouldEqual("b"); };
    }
}
