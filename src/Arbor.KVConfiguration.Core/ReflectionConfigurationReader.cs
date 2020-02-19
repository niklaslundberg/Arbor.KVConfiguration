using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions;
using Arbor.KVConfiguration.Core.Metadata;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    internal static class ReflectionConfigurationReader
    {
        public static ImmutableArray<KeyValueConfigurationItem> ReadConfiguration([NotNull] Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            ImmutableArray<ConfigurationMetadata> metadataFromAssemblyTypes = assembly.GetMetadataFromAssemblyTypes();

            var configurationItems =
                metadataFromAssemblyTypes
                    .Select(item => new KeyValueConfigurationItem(item.Key, item.DefaultValue, item))
                    .ToImmutableArray();

            return configurationItems;
        }
    }
}