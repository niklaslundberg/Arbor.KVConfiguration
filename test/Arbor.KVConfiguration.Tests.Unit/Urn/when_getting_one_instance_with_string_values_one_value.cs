using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_instance_with_string_values_one_value
    {
        private static IKeyValueConfiguration configuration;

        private static TypeWithStringValues instance;

        private Establish context = () =>
        {
            var keys = new NameValueCollection {{"urn:a:type:with:string:params:instance1:values", "value1"}};

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () => instance = (TypeWithStringValues)configuration.GetInstance(typeof(TypeWithStringValues))!;

        private It should_have_collection_length_1 = () => instance.Values.Length.ShouldEqual(1);

        private It should_have_one_value = () => instance.Values.ShouldContain("value1");

        private It should_not_be_null
            = () => instance.ShouldNotBeNull();
    }
}