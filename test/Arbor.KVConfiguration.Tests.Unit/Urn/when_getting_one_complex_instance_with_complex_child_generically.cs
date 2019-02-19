using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_complex_instance_with_complex_child_generically
    {
        private static IKeyValueConfiguration configuration;

        private static AComplexImmutableTypeWithComplexChild instance;

        private Establish context = () =>
        {
            var keys = new NameValueCollection
            {
                { "urn:test:a:complex:immutable:type-with-complex-child:instance1:id", "myId1" },
                { "urn:test:a:complex:immutable:type-with-complex-child:instance1:name", "myName1" },
                { "urn:test:a:complex:immutable:type-with-complex-child:instance1:child:name", "childName" },
                { "urn:test:a:complex:immutable:type-with-complex-child:instance1:child:count", "42" },
            };

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () => { instance = configuration.GetInstance<AComplexImmutableTypeWithComplexChild>(); };

        private It should_not_be_null
            = () => instance.ShouldNotBeNull();

        private It should_have_instance1_child =
            () => instance.Child.ShouldNotBeNull();

        private It should_have_instance1_child_name =
            () => instance.Child?.Name.ShouldEqual("childName");

        private It should_have_instance1_child_count =
            () => instance.Child?.Count.ShouldEqual(42);

        private It should_have_instance1_uri1 = () => { instance.Uri.ShouldBeNull(); };
    }
}
