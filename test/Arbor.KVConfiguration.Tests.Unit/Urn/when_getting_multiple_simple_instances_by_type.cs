using System.Collections.Immutable;
using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_multiple_simple_instances_by_type
    {
        private static IKeyValueConfiguration configuration;

        private static ImmutableArray<ASimpleType> instances;

        private Establish context = () =>
        {
            var primaryKeys = new NameValueCollection
            {
                {"urn:a:simple:type:instance1:url", "myUrl1"},
                {"urn:a:simple:type:instance1:text", "myText1"},
                {"unrelated1", "abc"}
            };

            var secondaryKeys = new NameValueCollection
            {
                {"urn:a:simple:type:instance2:url", ""},
                {"urn:a:simple:type:instance2:text", "myText2"},
                {"unrelated2", "123"}
            };

            var thirdKeys = new NameValueCollection
            {
                {"urn:a:simple:type:instance2:url", "myUrl2"}, {"urn:a:simple:type:instance2:text", ""}
            };

            configuration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(primaryKeys))
                .Add(new Core.InMemoryKeyValueConfiguration(secondaryKeys))
                .Add(new Core.InMemoryKeyValueConfiguration(thirdKeys))
                .Build();
        };

        private It first_instance_should_not_be_null
            = () => instances[0].ShouldNotBeNull();

        private Because of = () =>
        {
            instances = configuration.GetInstances(typeof(ASimpleType)).OfType<ASimpleType>().ToImmutableArray();
        };

        private It second_instance_should_not_be_null
            = () => instances[1].ShouldNotBeNull();

        private It should_have_2_instances = () => instances.Length.ShouldEqual(2);

        private It should_have_instance1_text = () =>
        {
            instances[0].Text.ShouldEqual("myText1");
        };

        private It should_have_instance1_url = () => { instances[0].Url.ShouldEqual("myUrl1"); };

        private It should_have_instance2_text = () =>
        {
            instances[1].Text.ShouldEqual("myText2");
        };

        private It should_have_instance2_url = () => { instances[1].Url.ShouldEqual("myUrl2"); };
    }
}