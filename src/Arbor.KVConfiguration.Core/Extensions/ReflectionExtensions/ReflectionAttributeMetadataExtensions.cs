using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Arbor.KVConfiguration.Core.Metadata;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions
{
    public static class ReflectionAttributeMetadataExtensions
    {
        public static ImmutableArray<ConfigurationMetadata> GetMetadataType(
            [NotNull] this TypeInfo typeInfo)
        {
            if (typeInfo == null)
            {
                throw new ArgumentNullException(nameof(typeInfo));
            }

            if (!typeInfo.IsPublicStaticClass())
            {
                return ImmutableArray<ConfigurationMetadata>.Empty;
            }

            return GetMetadataFromFields(typeInfo.GetPublicConstantStringFields());
        }

        public static ImmutableArray<ConfigurationMetadata> GetMetadataTypes(
            this ImmutableArray<TypeInfo> types)
        {
            if (types.IsDefaultOrEmpty)
            {
                return ImmutableArray<ConfigurationMetadata>.Empty;
            }

            return GetMetadataFromFields(types.SelectMany(type => type.GetPublicConstantStringFields())
                .ToImmutableArray());
        }

        public static ImmutableArray<ConfigurationMetadata> GetMetadataFromAssemblyTypes(
            [NotNull] this Assembly assembly)
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

            return GetMetadataFromFields(publicConstantPrimitiveFields);
        }

        private static ImmutableArray<ConfigurationMetadata> GetMetadataFromFields(ImmutableArray<FieldInfo> fields)
        {
            if (fields.IsDefaultOrEmpty)
            {
                return ImmutableArray<ConfigurationMetadata>.Empty;
            }

            var configurationMetadataFields = fields
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
                        new ConfigurationMetadata(pair.Field.GetValue(null) as string ??
                                                  "INVALID_VALUE_NOT_A_STRING",
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
