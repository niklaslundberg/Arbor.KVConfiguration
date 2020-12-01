using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.UserConfiguration;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Unit
{
    [Subject(typeof(UserJsonConfiguration))]
    public class when_getting_user_settings_when_it_does_not_exist
    {
        private static MultiSourceKeyValueConfiguration configuration;

        private Because of = () =>
            configuration = KeyValueConfigurationManager.Add(new UserJsonConfiguration()).Build();

        private It should_not_be_part_of_source_chain = () =>
            configuration.SourceChain.ShouldEqual(
                "source chain: Arbor.KVConfiguration.UserConfiguration.UserJsonConfiguration [no json file source]");
    }
}