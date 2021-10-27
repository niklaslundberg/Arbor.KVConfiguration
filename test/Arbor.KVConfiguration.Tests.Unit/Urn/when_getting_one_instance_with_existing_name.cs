using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_instance_with_existing_name
    {
        private static IKeyValueConfiguration configuration;

        private static TypeWithRequiredCtor? instance;

        private Establish context = () =>
        {
            var keys = new NameValueCollection {{"urn:type:with:required:ctor:instance1:key", "abc"}};

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () =>
        {
            instance = configuration.GetInstance(typeof(TypeWithRequiredCtor), "instance1") as TypeWithRequiredCtor;
        };

        private It should_not_be_null = () => instance.ShouldNotBeNull();
    }
}