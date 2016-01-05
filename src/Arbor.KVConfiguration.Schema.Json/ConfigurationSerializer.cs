using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Schema.Json
{
    public class ConfigurationSerializer
    {
        public Configuration Deserialize(string json)
        {
            Configuration configuration = JsonConvert.DeserializeObject<Configuration>(json);

            return configuration;
        }

        public string Serialize(Configuration configuration)
        {
            var json = JsonConvert.SerializeObject(
                configuration, 
                new JsonSerializerSettings { Formatting = Formatting.Indented });

            return json;
        }
    }
}