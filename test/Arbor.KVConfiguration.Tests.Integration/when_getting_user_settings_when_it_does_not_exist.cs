using Arbor.KVConfiguration.Core;
using Machine.Specifications;

namespace Arbor.KVConfiguration.Tests.Integration
{
    [Subject(typeof(UserConfiguration.UserConfiguration))]
    public class when_getting_user_settings_when_it_does_exist
    {
        private static MultiSourceKeyValueConfiguration configuration;

        private Because of = () =>
            configuration = KeyValueConfigurationManager.Add(new UserConfiguration.UserConfiguration()).Build();

        private It should_be_part_of_source_chain = () =>
            configuration.SourceChain.ShouldContain(
                @"\config.user', exists: True]");
    }
}
