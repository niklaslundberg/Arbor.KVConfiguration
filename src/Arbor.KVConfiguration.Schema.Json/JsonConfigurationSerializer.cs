using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static System.String;

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

            var configurationItems = JsonConvert.DeserializeObject<ConfigurationItems>(json);

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
