using System;
using System.Collections.Immutable;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions;
using Arbor.KVConfiguration.Core.Metadata;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Samples.ConsoleApp
{
    public class AppScenario5
    {
        public void Execute()
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
