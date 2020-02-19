using System;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public static class AppScenario5
    {
        public static void Execute()
        {
            KeyValueConfigurationManager
                .Add(new ReflectionKeyValueConfiguration(typeof(SampleConfigurationConstants).Assembly)).Build();

            ImmutableArray<ConfigurationMetadata> metadataFromAssemblyTypes =
                typeof(SampleConfigurationConstants).Assembly.GetMetadataFromAssemblyTypes();

            Console.WriteLine(JsonConvert.SerializeObject(metadataFromAssemblyTypes, Formatting.Indented));

            Console.WriteLine("Contains {0} keys", StaticKeyValueConfigurationManager.AppSettings.AllKeys.Length);

            foreach (StringPair stringPair in StaticKeyValueConfigurationManager.AppSettings.AllValues)
            {
                Console.WriteLine(stringPair);
            }
        }
    }
}