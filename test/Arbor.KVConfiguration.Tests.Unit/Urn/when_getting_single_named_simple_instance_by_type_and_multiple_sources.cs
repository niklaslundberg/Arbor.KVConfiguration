using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_single_named_simple_instance_by_type_and_multiple_sources
    {
        private static IKeyValueConfiguration configuration;

        private static ImmutableArray<INamedInstance<ASimpleType>> instances;

        private Establish context = () =>
        {
            var primaryKeys = new NameValueCollection
            {
                {"urn:a:simple:type2:instance1:url", "myUrl1"},
                {"urn:a:simple:type2:instance1:text", "myText1"},
                {"unrelated1", "abc"}
            };

            var secondaryKeys = new NameValueCollection
            {
                {"urn:a:simple:type:default1:url", "myUrl"},
                {"urn:a:simple:type:default1:text", "myText"},
                {"unrelated2", "123"}
            };

            var thirdKeys = new NameValueCollection {{"urn:a:simple:type3:instance2:url", ""}};

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
            instances = configuration.GetNamedInstances(typeof(ASimpleType))
                .Select(s => s as INamedInstance<ASimpleType>)
                .Where(s => s is {})
                .OfType<INamedInstance<ASimpleType>>()
                .ToImmutableArray();
        };

        private It should_have_1_instance = () => instances.Length.ShouldEqual(1);

        private It should_have_instance1_name = () => { instances[0].Name.ShouldEqual("default1"); };

        private It should_have_instance1_text = () =>
        {
            instances[0].Value.Text.ShouldEqual("myText");
        };

        private It should_have_instance1_url = () => { instances[0].Value.Url.ShouldEqual("myUrl"); };
    }
}