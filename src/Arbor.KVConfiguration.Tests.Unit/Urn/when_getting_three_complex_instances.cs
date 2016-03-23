using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;

using Machine.Specifications;

using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_three_complex_instances
    {
        private static IKeyValueConfiguration configuration;

        private static IReadOnlyCollection<AComplexImmutableType> instances;

        Establish context = () =>
            {
                var keys = new NameValueCollection
                               {
                                   { "urn:a:complex:immutable:type:instance1:id", "myId1" },
                                   { "urn:a:complex:immutable:type:instance1:name", "myName1" },
                                   { "urn:a:complex:immutable:type:instance1:children", "myChild1.1" },
                                   { "urn:a:complex:immutable:type:instance1:children", "myChild1.2" },
                                   { "urn:a:complex:immutable:type:instance2:id", "myId2" },
                                   { "urn:a:complex:immutable:type:instance2:name", "myName2" },
                                   { "urn:a:complex:immutable:type:instance3:id", "myId3" },
                                   { "urn:a:complex:immutable:type:instance3:name", "myName3" },
                                   { "urn:a:complex:immutable:type:instance3:children", "myChild3.1" },
                                   { "urn:a:complex:immutable:type:instance3:children", "myChild3.2" },
                                   { "urn:a:complex:immutable:type:instance3:children", "myChild3.3" },
                                   { "urn:a:complex:immutable:type:instance3:uri", "http://example.com/" }
                               };

                configuration = new Core.InMemoryKeyValueConfiguration(keys);
            };

        Because of = () => { instances = configuration.GetInstances<AComplexImmutableType>(); };

        It should_a_non_null_list
            = () => instances.ShouldNotBeNull();

        It should_return_a_list_with_3_elements = () =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(instances, Formatting.Indented));

                instances.Count.ShouldEqual(3);
            };

        It should_have_instance1_id1 = () =>
            {
                instances.OrderBy(i => i.Id).First().Id.ShouldEqual("myId1");
            };

        It should_have_instance1_name1 = () =>
            {
                instances.OrderBy(i => i.Id).First().Name.ShouldEqual("myName1");
            };

        It should_have_instance1_children1 = () =>
            {
                instances.OrderBy(i => i.Id).First().Children.ShouldContain("myChild1.1", "myChild1.2");
            };

        It should_have_instance1_uri1 = () =>
            {
                instances.OrderBy(i => i.Id).First().Uri.ShouldBeNull();
            };

        It should_have_instance2_id2 = () =>
            {
                instances.OrderBy(i => i.Id).Skip(1).First().Id.ShouldEqual("myId2");
            };

        It should_have_instance2_name2 = () =>
            {
                instances.OrderBy(i => i.Id).Skip(1).First().Name.ShouldEqual("myName2");
            };

        It should_have_instance2_children2 = () =>
            {
                instances.OrderBy(i => i.Id).Skip(1).First().Children.ShouldBeEmpty();
            };

        It should_have_instance2_uri2 = () =>
            {
                instances.OrderBy(i => i.Id).Skip(1).First().Uri.ShouldBeNull();
            };

        It should_have_instance3_id3 = () => {
            instances.OrderBy(i => i.Id).Skip(2).First().Id.ShouldEqual("myId3");
        };

        It should_have_instance3_name3 = () => {
            instances.OrderBy(i => i.Id).Skip(2).First().Name.ShouldEqual("myName3");
        };

        It should_have_instance3_children3 = () => {
            instances.OrderBy(i => i.Id).Skip(2).First().Children.ShouldContain("myChild3.1", "myChild3.2", "myChild3.3");
        };

        It should_have_instance3_uri3 = () => {
            instances.OrderBy(i => i.Id).Skip(2).First().Uri.ToString().ShouldEqual("http://example.com/");
        };

        It should_return_a_non_empty_list = () => instances.ShouldNotBeEmpty();
    }
}
