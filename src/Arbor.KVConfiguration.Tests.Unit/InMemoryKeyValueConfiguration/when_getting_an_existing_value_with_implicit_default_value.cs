using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.InMemoryKeyValueConfiguration
{
    [Subject(typeof(Core.InMemoryKeyValueConfiguration))]
    public class when_getting_an_existing_value_with_implicit_default_value
    {
        static Core.InMemoryKeyValueConfiguration configuration;

        static string value;

        Establish context = () =>
        {
            configuration = new Core.InMemoryKeyValueConfiguration(
                new NameValueCollection
                {
                    {"a", "b"}
                });
        };

        Because of = () => { value = configuration.ValueOrDefault("a"); };

        It return_existing_value = () => { value.ShouldEqual("b"); };
    }
}
