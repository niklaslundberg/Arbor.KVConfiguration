using System;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.SystemConfiguration;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public class AppScenario4
    {
        public void Execute()
        {
            KeyValueConfigurationManager.Initialize(
                new UserConfiguration.UserConfiguration(new AppSettingsKeyValueConfiguration()));

            Console.WriteLine("Contains {0} keys", KeyValueConfigurationManager.AppSettings.AllKeys.Length);

            foreach (StringPair stringPair in KeyValueConfigurationManager.AppSettings.AllValues)
            {
                Console.WriteLine(stringPair);
            }
        }
    }
}
