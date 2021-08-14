using System;
using System.Collections.Immutable;
using System.IO;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.JsonConfiguration;

namespace Arbor.KVConfiguration.UserConfiguration
{
    public class UserJsonConfiguration : IKeyValueConfiguration
    {
        private const string ConfigUserFileName = "config.user";

        private readonly IKeyValueConfiguration _configuration;
        private readonly string? _fileFullPath;

        public UserJsonConfiguration(string? basePath = null)
        {
            string? fileFullPath = TryGetConfigUser(basePath);

            if (!string.IsNullOrWhiteSpace(fileFullPath) && File.Exists(fileFullPath))
            {
                var jsonConfiguration = new JsonKeyValueConfiguration(fileFullPath);
                _configuration = jsonConfiguration!;
            }

            _configuration ??= NoConfiguration.Empty!;

            _fileFullPath = fileFullPath;
        }

        public ImmutableArray<string> AllKeys => _configuration.AllKeys;

        public ImmutableArray<StringPair> AllValues => _configuration.AllValues;

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
            => _configuration.AllWithMultipleValues;

        public string this[string? key]
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

        private static string? TryGetConfigUser(string? basePath)
        {
            basePath ??= AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                string fileFullPath =
                    Path.Combine(basePath, ConfigUserFileName);

                var fileInfo = new FileInfo(fileFullPath);

                var currentDirectory = fileInfo.Directory;

                while (currentDirectory is {})
                {
                    var configUserFiles = currentDirectory.GetFiles(ConfigUserFileName);

                    if (configUserFiles.Length == 1)
                    {
                        return configUserFiles[0].FullName;
                    }

                    currentDirectory = currentDirectory.Parent;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
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