using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using System.Collections.Immutable;

namespace Arbor.KVConfiguration.Schema
{
    public class AttributeMetadataSource
    {
        public ImmutableArray<ConfigurationMetadata> GetMetadataFromAssemblyTypes([NotNull] Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (assembly.IsDynamic)
            {
                return ImmutableArray<ConfigurationMetadata>.Empty;
            }

            ImmutableArray<FieldInfo> publicConstantPrimitiveFields = assembly.GetPublicConstantStringFields();

            if (!publicConstantPrimitiveFields.Any())
            {
                return ImmutableArray<ConfigurationMetadata>.Empty;
            }

            var configurationMetadataFields = publicConstantPrimitiveFields
                .Select(
                    field => new
                    {
                        Field = field,
                        Attribute = field.GetCustomAttribute<MetadataAttribute>()
                    })
                .Where(pair => pair.Attribute != null)
                .ToArray();

            if (!configurationMetadataFields.Any())
            {
                return ImmutableArray<ConfigurationMetadata>.Empty;
            }

            ImmutableArray<ConfigurationMetadata> metadata = configurationMetadataFields
                .Select(
                    pair =>
                        new ConfigurationMetadata(pair.Field.GetRawConstantValue() as string ?? "INVALID_VALUE_NOT_A_STRING",
                            pair.Attribute.ValueType,
                            pair.Field.Name,
                            pair.Attribute.Description,
                            pair.Attribute.PartInvariantName,
                            pair.Attribute.PartFullName,
                            pair.Field.DeclaringType,
                            pair.Attribute.SourceLine,
                            pair.Attribute.SourceFile,
                            pair.Attribute.IsRequired,
                            pair.Attribute.DefaultValue,
                            pair.Attribute.Notes,
                            pair.Attribute.AllowMultipleValues,
                            pair.Attribute.Examples,
                            pair.Attribute.Tags,
                            pair.Attribute.KeyType))
                .ToImmutableArray();

            return metadata;
        }
    }
}
