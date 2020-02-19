using System;
using System.Reflection;
using Arbor.KVConfiguration.Core.Metadata;

namespace Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions
{
    public static class ReflectionAttributeMetadataExtensions
    {
        public static ImmutableArray<ConfigurationMetadata> GetMetadataType(
            [NotNull] this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsPublicStaticClass())
            {
                return ImmutableArray<ConfigurationMetadata>.Empty;
            }

            return GetMetadataFromFields(type.GetPublicConstantStringFields());
        }

        public static ImmutableArray<ConfigurationMetadata> GetMetadataTypes(
            this ImmutableArray<Type> types)
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
            if (assembly is null)
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
                    field => new {Field = field, Attribute = field.GetCustomAttribute<MetadataAttribute>()})
                .Where(pair => pair.Attribute is object)
                .ToArray();

            if (!configurationMetadataFields.Any())
            {
                return ImmutableArray<ConfigurationMetadata>.Empty;
            }

            var metadata = configurationMetadataFields
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