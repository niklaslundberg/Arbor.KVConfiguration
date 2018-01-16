using System.Collections.Immutable;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_multiple_named_simple_instances_by_generic_type_and_multiple_sources
    {
        private static IKeyValueConfiguration configuration;

        private static ImmutableArray<INamedInstance<ASimpleType>> instances;

        private Establish context = () =>
        {
            var primaryKeys = new NameValueCollection
            {
                { "urn:a:simple:type2:instance1:url", "myUrl1" },
                { "urn:a:simple:type2:instance1:text", "myText1" },
                { "urn:a:simple:type:default1:url", "myUrl1" },
                { "urn:a:simple:type:default1:text", "myText1" },
                { "unrelated1", "abc" },
            };

            var secondaryKeys = new NameValueCollection
            {
                { "urn:a:simple:type:default0:url", "myUrl0" },
                { "urn:a:simple:type:default0:text", "myText0" },
                { "unrelated2", "123" },
            };

            var thirdKeys = new NameValueCollection
            {
                { "urn:a:simple:type3:instance2:url", "" }
            };

            configuration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(primaryKeys))
                .Add(new Core.InMemoryKeyValueConfiguration(secondaryKeys))
                .Add(new Core.InMemoryKeyValueConfiguration(thirdKeys))
                .Build();
        };

        private Because of = () =>
        {
            instances = configuration.GetNamedInstances<ASimpleType>()
                .ToImmutableArray();
        };

        private It first_instance_should_not_be_null
            = () => instances[0].ShouldNotBeNull();

        private It should_have_instance1_url = () => { instances[0].Value.Url.ShouldEqual("myUrl0"); };

        private It should_have_instance1_name = () => { instances[0].Name.ShouldEqual("default0"); };

        private It should_have_instance1_text = () =>
        {
            instances[0].Value.Text.ShouldEqual("myText0");
        };

        private It second_instance_should_not_be_null
            = () => instances[1].ShouldNotBeNull();

        private It second_have_instance2_url = () => { instances[1].Value.Url.ShouldEqual("myUrl1"); };

        private It second_have_instance2_name = () => { instances[1].Name.ShouldEqual("default1"); };

        private It second_have_instance2_text = () =>
        {
            instances[1].Value.Text.ShouldEqual("myText1");
        };

        private It should_have_2_instances = () => instances.Length.ShouldEqual(2);
    }
}