﻿using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using Arbor.KVConfiguration.Core;
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
                throw new ArgumentException(KeyValueResources.ArgumentIsNullOrWhitespace, nameof(fileFullPath));
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

            ConfigurationItems config;

            try
            {
                config = JsonConfigurationSerializer.Deserialize(json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Could not read JSON configuration from file path '{_fileFullPath}' and JSON '{json}'", ex);
            }

            return config;
        }

        public ImmutableArray<KeyValueConfigurationItem> ReadConfiguration() =>
            GetConfigurationItems().ReadConfiguration();
    }
}