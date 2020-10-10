using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.UserConfiguration;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration.MSpec
{
    [Subject(typeof(UserJsonConfiguration))]
    public class when_getting_user_settings_when_it_does_exist
    {
        static MultiSourceKeyValueConfiguration configuration;

        Because of = () =>
            configuration = KeyValueConfigurationManager.Add(new UserJsonConfiguration()).Build();

        It should_be_part_of_source_chain = () =>
            configuration.SourceChain.ShouldContain(
                @"\config.user', exists: True]");
    }
}