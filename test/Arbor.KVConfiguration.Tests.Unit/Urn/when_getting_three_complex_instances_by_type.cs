using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_three_complex_instances_by_type
    {
        private static IKeyValueConfiguration configuration;

        private static ImmutableArray<AComplexImmutableType> instances;

        private Establish context = () =>
        {
            var keys = new NameValueCollection
            {
                {"urn:test:a:complex:immutable:type:instance1:id", "myId1"},
                {"urn:test:a:complex:immutable:type:instance1:name", "myName1"},
                {"urn:test:a:complex:immutable:type:instance1:children", "myChild1.1"},
                {"urn:test:a:complex:immutable:type:instance1:children", "myChild1.2"},
                {"urn:test:a:complex:immutable:type:instance2:id", "myId2"},
                {"urn:test:a:complex:immutable:type:instance2:name", "myName2"},
                {"urn:test:a:complex:immutable:type:instance3:id", "myId3"},
                {"urn:test:a:complex:immutable:type:instance3:name", "myName3"},
                {"urn:test:a:complex:immutable:type:instance3:children", "myChild3.1"},
                {"urn:test:a:complex:immutable:type:instance3:children", "myChild3.2"},
                {"urn:test:a:complex:immutable:type:instance3:children", "myChild3.3"},
                {"urn:test:a:complex:immutable:type:instance3:uri", "http://example.com/"}
            };

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () =>
        {
            instances = configuration.GetInstances(typeof(AComplexImmutableType)).OfType<AComplexImmutableType>()
                .ToImmutableArray();
        };

        private It should_be_a_non_empty_list
            = () => instances.ShouldNotBeEmpty();

        private It should_have_instance1_children1 =
            () => instances.OrderBy(i => i.Id).First().Children.ShouldContain("myChild1.1", "myChild1.2");

        private It should_have_instance1_id1 = () => instances.OrderBy(i => i.Id).First().Id.ShouldEqual("myId1");

        private It should_have_instance1_name1 = () => instances.OrderBy(i => i.Id).First().Name.ShouldEqual("myName1");

        private It should_have_instance1_uri1 = () => instances.OrderBy(i => i.Id).First().Uri.ShouldBeNull();

        private It should_have_instance2_children2 =
            () => instances.OrderBy(i => i.Id).Skip(1).First().Children.ShouldBeEmpty();

        private It should_have_instance2_id2 = () => instances.OrderBy(i => i.Id).Skip(1).First().Id.ShouldEqual("myId2");

        private It should_have_instance2_name2 =
            () => instances.OrderBy(i => i.Id).Skip(1).First().Name.ShouldEqual("myName2");

        private It should_have_instance2_uri2 = () => instances.OrderBy(i => i.Id).Skip(1).First().Uri.ShouldBeNull();

        private It should_have_instance3_children3 =
            () =>
            {
                instances.OrderBy(i => i.Id)
                    .Skip(2)
                    .First()
                    .Children.ShouldContain("myChild3.1", "myChild3.2", "myChild3.3");
            };

        private It should_have_instance3_id3 = () => instances.OrderBy(i => i.Id).Skip(2).First().Id.ShouldEqual("myId3");

        private It should_have_instance3_name3 =
            () => instances.OrderBy(i => i.Id).Skip(2).First().Name.ShouldEqual("myName3");

        private It should_have_instance3_uri3 =
            () => instances.OrderBy(i => i.Id).Skip(2).First().Uri.ToString().ShouldEqual("http://example.com/");

        private It should_return_a_list_with_3_elements = () =>
        {
            Console.WriteLine(JsonConvert.SerializeObject(instances, Formatting.Indented));

            instances.Length.ShouldEqual(3);
        };

        private It should_return_a_non_empty_list = () => instances.ShouldNotBeEmpty();
    }
}