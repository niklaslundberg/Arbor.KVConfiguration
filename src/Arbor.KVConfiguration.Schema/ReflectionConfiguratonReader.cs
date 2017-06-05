﻿using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public static class ReflectionConfiguratonReader
    {
        public static ImmutableArray<KeyValueConfigurationItem> ReadConfiguration([NotNull] Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            ImmutableArray<ConfigurationMetadata> metadataFromAssemblyTypes =
                AttributeMetadataSource.GetMetadataFromAssemblyTypes(assembly);

            ImmutableArray<KeyValueConfigurationItem> configurationItems =
                metadataFromAssemblyTypes
                    .Select(item => new KeyValueConfigurationItem(item.Key, item.DefaultValue, item))
                    .ToImmutableArray();

            return configurationItems;
        }
    }
}