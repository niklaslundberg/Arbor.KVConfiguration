using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Arbor.KVConfiguration.Schema;
using Arbor.KVConfiguration.Schema.Json;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    public class JsonFileReader
    {
        private readonly string _fileFullPath;

        public JsonFileReader(string fileFullPath)
        {
            _fileFullPath = fileFullPath;
        }

        public IReadOnlyCollection<KeyValueConfigurationItem> ReadConfiguration()
        {
            string json = File.ReadAllText(_fileFullPath, Encoding.UTF8);

            Configuration config = new ConfigurationSerializer().Deserialize(json);

            return
                config.Keys.Select(
                    keyValue => new KeyValueConfigurationItem(keyValue.Key, keyValue.Value, keyValue.Metadata)).ToList();
        }
    }
}