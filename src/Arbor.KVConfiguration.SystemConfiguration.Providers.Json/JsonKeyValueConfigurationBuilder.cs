using System;
using System.Collections.Specialized;
using System.IO;
using Arbor.KVConfiguration.Core;
using Microsoft.Configuration.ConfigurationBuilders;

namespace Arbor.KVConfiguration.SystemConfiguration.Providers.Json
{
    public class JsonKeyValueConfigurationBuilder : KeyValueConfigurationBuilder
    {
        private const string JsonFilePropertyName = "jsonFile";
        private const string IgnoreMissingFilePropertyName = "ignoreMissingFile";

        private bool _ignoreMissingFile;
        private string _jsonFilePath;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(JsonKeyValueConfigurationBuilder)} is missing value for its 'name' property", nameof(name));
            }

            _ignoreMissingFile = !bool.TryParse(config[IgnoreMissingFilePropertyName], out bool result) || result;

            string jsonFile = config[JsonFilePropertyName];

            if (string.IsNullOrWhiteSpace(jsonFile))
            {
                throw new ArgumentException(
                    $"{nameof(JsonKeyValueConfigurationBuilder)} '{name}': Json file must be specified with the '{JsonFilePropertyName}' attribute.");
            }

            _jsonFilePath = Utils.MapPath(jsonFile);

            if (!_ignoreMissingFile && !File.Exists(_jsonFilePath))
            {
                throw new ArgumentException(
                    $"{nameof(JsonKeyValueConfigurationBuilder)} '{name}': Json file does not exist.");
            }

            base.Initialize(name, config);
        }

        protected override IKeyValueConfiguration GetKeyValueConfiguration()
        {
            var jsonConfiguration = new JsonConfiguration.JsonKeyValueConfiguration(_jsonFilePath, throwWhenNotExists: !_ignoreMissingFile);

            return jsonConfiguration;
        }
    }
}
