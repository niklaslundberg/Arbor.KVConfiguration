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

        public UserConfiguration()
        {
            string fileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigUserFileName);

            if (File.Exists(fileFullPath))
            {
                var jsonConfiguration = new JsonKeyValueConfiguration(fileFullPath);
                _configuration = jsonConfiguration;
            }
            else
            {
                var fileInfo = new FileInfo(fileFullPath);

                string parentName = fileInfo.Directory?.Parent?.Name ?? string.Empty;

                if (parentName.Equals("bin"))
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
                _configuration = new InMemoryKeyValueConfiguration(new NameValueCollection());
            }
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
    }
}
