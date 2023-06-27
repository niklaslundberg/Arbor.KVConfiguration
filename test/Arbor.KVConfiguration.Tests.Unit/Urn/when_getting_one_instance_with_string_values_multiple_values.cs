using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_instance_with_string_values_multiple_values
    {
        private static IKeyValueConfiguration configuration;

        private static TypeWithStringValues? instance;

        private Establish context = () =>
        {
            var keys = new NameValueCollection
            {
                {"urn:a:type:with:string:params:instance1:values", "value1"},
                {"urn:a:type:with:string:params:instance1:values", "value2"}
            };

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () => instance = configuration.GetInstance(typeof(TypeWithStringValues)) as TypeWithStringValues;

        private It should_have_multiple_values = () => instance.Values.ShouldContain("value1", "value2");

        private It should_not_be_null
            = () => instance.ShouldNotBeNull();
    }
}