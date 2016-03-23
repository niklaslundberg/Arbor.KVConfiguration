using System;
using static System.String;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class JsonConfigurationSerializer
    {
        public ConfigurationItems Deserialize(string json)
        {
            if (IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(json));
            }

            ConfigurationItems configurationItems = JsonConvert.DeserializeObject<ConfigurationItems>(json);

            return configurationItems;
        }

        public string Serialize(ConfigurationItems configurationItems)
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
