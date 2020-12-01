using System.Collections.Specialized;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit.Urn
{
    [Subject(typeof(UrnKeyValueExtensions))]
    public class when_invoking_source_chain
    {
        private static MultiSourceKeyValueConfiguration configuration;

        private static string chain;

        private Establish context = () =>
        {
            var a = new NameValueCollection();

            var b = new NameValueCollection();

            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager
                .Add(new Core.InMemoryKeyValueConfiguration(a, "B"))
                .Add(new Core.InMemoryKeyValueConfiguration(b, "A"))
                .Build();

            configuration = multiSourceKeyValueConfiguration;
        };

        private Because of = () =>
        {
            chain = configuration.SourceChain;
        };

        private It should_return_the_chain_as_string = () =>
        {
            chain.ShouldEqual(
                "source chain: Arbor.KVConfiguration.Core.InMemoryKeyValueConfiguration [name: 'A']-->Arbor.KVConfiguration.Core.InMemoryKeyValueConfiguration [name: 'B']");
        };
    }
}