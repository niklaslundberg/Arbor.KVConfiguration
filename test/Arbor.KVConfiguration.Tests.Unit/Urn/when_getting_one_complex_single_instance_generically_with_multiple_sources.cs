using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_complex_single_instance_generically_with_multiple_sources
    {
        private static IKeyValueConfiguration configuration;

        private static AComplexImmutableType? instance;

        private Establish context = () =>
        {
            var userKeys = new NameValueCollection
            {
                {"urn:test:a:complex:immutable:type:instance1:name", "myName1"},
                {"urn:test:a:complex:immutable:type:instance1:id", "myId1"},
                {"urn:test:a:complex:immutable:type:instance1:children", "myChild1.1"},
                {"urn:test:a:complex:immutable:type:instance1:children", "myChild1.2"}
            };

            var baseKeys = new NameValueCollection {{"urn:test:unrelated", "true"}};

            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(baseKeys))
                .Add(new Core.InMemoryKeyValueConfiguration(userKeys))
                .Build();

            configuration = multiSourceKeyValueConfiguration;
        };

        private Because of = () => { instance = configuration.GetInstance<AComplexImmutableType>(); };

        private It should_have_instance1_children1 =
            () => { instance.Children.ShouldContain("myChild1.1", "myChild1.2"); };

        private It should_have_instance1_id1 = () => { instance.Id.ShouldEqual("myId1"); };

        private It should_have_instance1_name1 = () => { instance.Name.ShouldEqual("myName1"); };

        private It should_have_instance1_uri1 = () => { instance.Uri.ShouldBeNull(); };

        private It should_not_be_null
            = () => instance.ShouldNotBeNull();
    }
}