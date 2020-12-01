using System;
using Arbor.KVConfiguration.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static System.String;

namespace Arbor.KVConfiguration.Schema.Json
{
    public static class JsonConfigurationSerializer
    {
        public static ConfigurationItems Deserialize(string json)
        {
            if (IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException(KeyValueResources.ArgumentIsNullOrWhitespace, nameof(json));
            }

            var configurationItems = JsonConvert.DeserializeObject<ConfigurationItems>(json);

            return configurationItems;
        }

        public static string Serialize(ConfigurationItems configurationItems)
        {
            string json = JsonConvert.SerializeObject(
                configurationItems,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            return json;
        }
    }
}