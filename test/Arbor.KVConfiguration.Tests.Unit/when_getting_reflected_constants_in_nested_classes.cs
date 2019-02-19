using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Metadata;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(ReflectionKeyValueConfiguration))]
    public class when_getting_reflected_constants_in_nested_classes
    {
        private Establish context = () =>
        {
            reflection_key_value_configuration =
                new ReflectionKeyValueConfiguration(typeof(ClassWithNestedClasses).Assembly);
        };

        private Because of = () =>
        {
            configuration_items = reflection_key_value_configuration.ConfigurationItems;
        };

        private It should_find_plain_key = () =>
        {
            configuration_items.FirstOrDefault(x => x.Key == ClassWithNestedClasses.PlainKey).Value.ShouldNotBeNull();
        };

        private It should_find_nested_key = () =>
        {
            configuration_items.FirstOrDefault(x => x.Key == ClassWithNestedClasses.NestedA.NestedKeyA).Value.ShouldNotBeNull();
        };

        private It should_find_second_nested_key = () =>
        {
            configuration_items.FirstOrDefault(x => x.Key == ClassWithNestedClasses.NestedB.NestedKeyB).Value.ShouldNotBeNull();
        };

        private static ReflectionKeyValueConfiguration reflection_key_value_configuration;

        private static ImmutableArray<KeyValueConfigurationItem> configuration_items;
    }
}
