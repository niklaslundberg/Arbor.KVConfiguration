using System;

using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.SystemConfiguration;
using Arbor.KVConfiguration.UserConfiguration;

namespace Arbor.KVConfigurat.Samples.ConsoleApp
{
    public class AppScenario4
    {
        public void Execute()
        {
            KVConfigurationManager.Initialize(new UserConfiguration(new AppSettingsKeyValueConfiguration()));

            Console.WriteLine("Contains {0} keys", KVConfigurationManager.AppSettings.AllKeys.Count);

            foreach (StringPair stringPair in KVConfigurationManager.AppSettings.AllValues)
            {
                Console.WriteLine(stringPair);
            }
        }
    }
}
