using System;
using System.Collections.Specialized;
using System.IO;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.JsonConfiguration;
using Microsoft.Configuration.ConfigurationBuilders;

namespace Arbor.KVConfiguration.SystemConfiguration.Providers.Json
{
    public class JsonKeyValueConfigurationBuilder : KeyValueConfigurationBuilder
    {
        private const string JsonFilePropertyName = "jsonFile";
        private const string IgnoreMissingFilePropertyName = "ignoreMissingFile";

        private bool _ignoreMissingFile;
        private string? _jsonFilePath;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(JsonKeyValueConfigurationBuilder)} is missing value for its 'name' property",
                    nameof(name));
            }

            _ignoreMissingFile = !bool.TryParse(config[IgnoreMissingFilePropertyName], out bool result) || result;

            string jsonFile = config[JsonFilePropertyName];

            if (string.IsNullOrWhiteSpace(jsonFile))
            {
                throw new ArgumentException(
                    $"{nameof(JsonKeyValueConfigurationBuilder)} '{name}': Json file must be specified with the '{JsonFilePropertyName}' attribute.");
            }

            string jsonFilePath = Utils.MapPath(jsonFile);

            if (string.IsNullOrWhiteSpace(jsonFilePath))
            {
                throw new InvalidOperationException($"Could not get path for json file {jsonFile}");
            }

            _jsonFilePath = jsonFilePath;

            if (!_ignoreMissingFile && !File.Exists(_jsonFilePath))
            {
                throw new ArgumentException(
                    $"{nameof(JsonKeyValueConfigurationBuilder)} '{name}': Json file does not exist.");
            }

            base.Initialize(name, config);
        }

        protected override IKeyValueConfiguration GetKeyValueConfiguration()
        {
            if (string.IsNullOrWhiteSpace(_jsonFilePath))
            {
                throw new InvalidOperationException("Json file path is not set");
            }

            var jsonConfiguration = new JsonKeyValueConfiguration(_jsonFilePath!, !_ignoreMissingFile);

            return jsonConfiguration;
        }
    }
}