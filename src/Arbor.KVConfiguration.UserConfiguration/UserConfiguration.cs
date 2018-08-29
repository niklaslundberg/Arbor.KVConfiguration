using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.IO;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.JsonConfiguration;

namespace Arbor.KVConfiguration.UserConfiguration
{
    public class UserConfiguration : IKeyValueConfiguration
    {
        private const string ConfigUserFileName = "config.user";

        private readonly IKeyValueConfiguration _configuration;
        private readonly string _fileFullPath;

        public UserConfiguration(string basePath = null)
        {
            string fileFullPath = Path.Combine(basePath ?? AppDomain.CurrentDomain.BaseDirectory, ConfigUserFileName);

            if (File.Exists(fileFullPath))
            {
                var jsonConfiguration = new JsonKeyValueConfiguration(fileFullPath);
                _configuration = jsonConfiguration;
            }
            else
            {
                var fileInfo = new FileInfo(fileFullPath);

                string parentName = fileInfo.Directory?.Parent?.Name ?? string.Empty;

                if (parentName.Equals("bin", StringComparison.OrdinalIgnoreCase))
                {
                    string projectDirectoryFullDirectoryPath =
                        fileInfo.Directory?.Parent?.Parent?.FullName ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(projectDirectoryFullDirectoryPath))
                    {
                        string projectDirectoryFullFilePath = Path.Combine(
                            projectDirectoryFullDirectoryPath,
                            ConfigUserFileName);

                        if (File.Exists(projectDirectoryFullFilePath))
                        {
                            var jsonConfiguration = new JsonKeyValueConfiguration(projectDirectoryFullFilePath);
                            _configuration = jsonConfiguration;
                        }
                    }
                }
            }

            if (_configuration == null)
            {
                _configuration = NoConfiguration.Empty;
            }

            _fileFullPath = fileFullPath;
        }

        public ImmutableArray<string> AllKeys => _configuration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _configuration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
            => _configuration.AllWithMultipleValues;

        public string this[string key]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return string.Empty;
                }

                return _configuration[key];
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(_fileFullPath))
            {
                return $"{base.ToString()} [json file source: '{_fileFullPath}', exists: {File.Exists(_fileFullPath)}]";
            }

            return $"{base.ToString()} [no json file source]";
        }
    }
}
