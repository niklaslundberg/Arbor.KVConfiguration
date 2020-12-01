using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_instance_with_string_zero_values
    {
        private static IKeyValueConfiguration configuration;

        private static TypeWithStringValues instance;

        private Establish context = () =>
        {
            var keys = new NameValueCollection {{"urn:a:type:with:string:params:instance1:other", "def"}};

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () =>
        {
            instance = configuration.GetInstance(typeof(TypeWithStringValues)) as TypeWithStringValues;
        };

        private It should_have_collection_length_0 = () => instance.Values.Length.ShouldEqual(0);

        private It should_not_be_null
            = () => instance.ShouldNotBeNull();
    }
}