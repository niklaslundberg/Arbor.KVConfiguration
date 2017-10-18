using System;
using System.IO;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.SystemConfiguration.Providers
{
    public class JsonKeyValueConfigurationBuilder : KeyValueConfigurationBuilder
    {
        protected override IKeyValueConfiguration GetKeyValueConfiguration()
        {
            var jsonConfiguration = new JsonConfiguration.JsonKeyValueConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json"), throwWhenNotExists: false);

            return jsonConfiguration;
        }
    }
}
