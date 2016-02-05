using System.Collections.Specialized;

using Arbor.KVConfiguration.Core;

using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.InMemoryKeyValueConfiguration
{
    public class when_getting_a_non_existing_value_with_explicit_default_value
    {
        private static Core.InMemoryKeyValueConfiguration configuration;

        private static string value;

        private Establish context = () => {
                                              configuration = new Core.InMemoryKeyValueConfiguration(
                                                  new NameValueCollection {
                                                          { "a", "b" }
                                                      });
        };

        private Because of = () => { value = configuration.ValueOrDefault("d", "e"); };

        private It return_existing_value = () => { value.ShouldEqual("e"); };
    }
}
