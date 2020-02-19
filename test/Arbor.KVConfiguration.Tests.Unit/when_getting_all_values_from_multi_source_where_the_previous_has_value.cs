using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(MultiSourceKeyValueConfiguration))]
    public class when_getting_all_values_from_multi_source_where_the_previous_has_value
    {
        private static MultiSourceKeyValueConfiguration multi_source_key_value_configuration;
        private static ImmutableArray<StringPair> found_value;

        private Establish context = () =>
        {
            var userKeys = new NameValueCollection
            {
                {"urn:test:a:complex:immutable:type:instance1:id", "myId1"},
                {"urn:test:a:complex:immutable:type:instance1:name", "myName1"},
                {"urn:test:a:complex:immutable:type:instance1:children", "myChild1.1"},
                {"urn:test:a:complex:immutable:type:instance1:children", "myChild1.2"}
            };

            var baseKeys = new NameValueCollection {{"urn:unrelated", "true"}};

            multi_source_key_value_configuration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(baseKeys))
                .Add(new Core.InMemoryKeyValueConfiguration(userKeys))
                .Build();
        };

        private Because of = () =>
            found_value = multi_source_key_value_configuration.AllValues;

        private It should_find_the_previous_value = () =>
            found_value.Select(s => s.Value).ShouldContain("true", "myId1", "myName1", "myChild1.1,myChild1.2");
    }
}