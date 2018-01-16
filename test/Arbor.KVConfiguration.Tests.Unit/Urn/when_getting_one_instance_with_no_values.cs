using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_getting_one_instance_with_no_values
    {
        private static IKeyValueConfiguration configuration;

        private static TypeWithRequiredCtor instance;

        private Establish context = () =>
        {
            var keys = new NameValueCollection
            {
            };

            configuration = new Core.InMemoryKeyValueConfiguration(keys);
        };

        private Because of = () =>
        {
            instance = configuration.GetInstance(typeof(TypeWithRequiredCtor)) as TypeWithRequiredCtor;
        };

        private It should_be_null = () => instance.ShouldBeNull();
    }
}