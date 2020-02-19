using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_complex_instance_with_complex_children_generically
    {
        private static IKeyValueConfiguration configuration;

        private static AComplexImmutableTypeWithComplexChildren instance;

        private Establish context = () =>
        {
            var keys = new NameValueCollection
            {
                {"urn:test:a:complex:immutable:type-with-complex-children:instance1:id", "myId1"},
                {"urn:test:a:complex:immutable:type-with-complex-children:instance1:name", "myName1"},
                {
                    "urn:test:a:complex:immutable:type-with-complex-children:instance1:children:child1:name",
                    "child1Name"
                },
                {"urn:test:a:complex:immutable:type-with-complex-children:instance1:children:child1:count", "42"},
                {
                    "urn:test:a:complex:immutable:type-with-complex-children:instance1:children:child2:name",
                    "child2Name"
                },
                {"urn:test:a:complex:immutable:type-with-complex-children:instance1:children:child2:count", "-42"}
            };

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () =>
        {
            instance = configuration.GetInstance<AComplexImmutableTypeWithComplexChildren>();
        };

        private It should_have_instance1_children1 =
            () => instance.Children[0]?.ShouldNotBeNull();

        private It should_have_instance1_children1_count =
            () => instance.Children[0]?.Count.ShouldEqual(42);

        private It should_have_instance1_children1_name =
            () => instance.Children[0]?.Name?.ShouldEqual("child1Name");

        private It should_have_instance1_children2 =
            () => instance.Children[1]?.ShouldNotBeNull();

        private It should_have_instance1_children2_count =
            () => instance.Children[1]?.Count.ShouldEqual(-42);

        private It should_have_instance1_children2_name =
            () => instance.Children[1]?.Name?.ShouldEqual("child2Name");

        private It should_have_instance1_id1 = () => { instance.Id.ShouldEqual("myId1"); };

        private It should_have_instance1_name1 = () =>
        {
            instance.Name.ShouldEqual("myName1");
        };

        private It should_have_instance1_uri1 = () => { instance.Uri.ShouldBeNull(); };

        private It should_not_be_null
            = () => instance.ShouldNotBeNull();
    }
}