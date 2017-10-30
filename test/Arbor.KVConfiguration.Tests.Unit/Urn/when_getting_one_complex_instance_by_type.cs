using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_complex_instance_by_type
    {
        private static IKeyValueConfiguration configuration;

        private static AComplexImmutableType instance;

        private Establish context = () =>
        {
            var keys = new NameValueCollection
            {
                { "urn:a:complex:immutable:type:instance1:id", "myId1" },
                { "urn:a:complex:immutable:type:instance1:name", "myName1" },
                { "urn:a:complex:immutable:type:instance1:children", "myChild1.1" },
                { "urn:a:complex:immutable:type:instance1:children", "myChild1.2" },
            };

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () => { instance = configuration.GetInstance(typeof(AComplexImmutableType)) as AComplexImmutableType; };

        private It should_a_non_null_list
            = () => instance.ShouldNotBeNull();

        private It should_have_instance1_children1 =
            () => { instance.Children.ShouldContain("myChild1.1", "myChild1.2"); };

        private It should_have_instance1_id1 = () => { instance.Id.ShouldEqual("myId1"); };

        private It should_have_instance1_name1 = () =>
        {
            instance.Name.ShouldEqual("myName1");
        };

        private It should_have_instance1_uri1 = () => { instance.Uri.ShouldBeNull(); };
    }
}