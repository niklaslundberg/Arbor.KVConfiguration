using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Schema.Json;

namespace Arbor.KVConfiguration.JsonConfiguration
{
    public class JsonFileReader
    {
        private readonly string _fileFullPath;

        public JsonFileReader(string fileFullPath)
        {
            if (string.IsNullOrWhiteSpace(fileFullPath))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(fileFullPath));
            }

            if (!File.Exists(fileFullPath))
            {
                throw new FileNotFoundException("The file does not exist", fileFullPath);
            }

            _fileFullPath = fileFullPath;
        }

        public ConfigurationItems GetConfigurationItems()
        {
            string json = File.ReadAllText(_fileFullPath, Encoding.UTF8);

            ConfigurationItems config = new JsonConfigurationSerializer().Deserialize(json);

            return config;
        }

        public ImmutableArray<KeyValueConfigurationItem> ReadConfiguration()
        {
            return GetConfigurationItems().ReadConfiguration();
        }
    }
}
