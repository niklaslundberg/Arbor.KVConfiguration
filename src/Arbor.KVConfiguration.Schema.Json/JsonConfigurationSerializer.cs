using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class JsonConfigurationSerializer
    {
        public ConfigurationItems Deserialize(string json)
        {
            ConfigurationItems configurationItems = JsonConvert.DeserializeObject<ConfigurationItems>(json);

            return configurationItems;
        }

        public string Serialize(ConfigurationItems configurationItems)
        {
            var json = JsonConvert.SerializeObject(
                configurationItems, 
                new JsonSerializerSettings { Formatting = Formatting.Indented });

            return json;
        }
    }
}
