using System;
using System.Collections.Generic;
using System.IO;
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
            if (string.IsNullOrWhiteSpace(fileFullPath))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(fileFullPath));
            }

            _fileFullPath = fileFullPath;
        }

        public ConfigurationItems GetConfigurationItems()
        {
            string json = File.ReadAllText(_fileFullPath, Encoding.UTF8);

            ConfigurationItems config = new JsonConfigurationSerializer().Deserialize(json);

            return config;
        }

        public IReadOnlyCollection<KeyValueConfigurationItem> ReadConfiguration()
        {
            return GetConfigurationItems().ReadConfiguration();
        }
    }
}
