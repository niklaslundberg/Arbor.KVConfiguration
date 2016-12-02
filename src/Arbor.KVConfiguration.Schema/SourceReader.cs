using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public class SourceReader
    {

        public IReadOnlyCollection<KeyValueConfigurationItem> ReadConfiguration([NotNull] Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            IReadOnlyCollection<Metadata> metadataFromAssemblyTypes = new AttributeMetadataSource().GetMetadataFromAssemblyTypes(assembly);

            KeyValueConfigurationItem[] configurationItems = metadataFromAssemblyTypes.Select(item => new KeyValueConfigurationItem(item.Key, item.DefaultValue, item)).ToArray();

            return configurationItems;
        }
    }
}