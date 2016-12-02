using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    public class AttributeMetadataSource
    {
        public IReadOnlyCollection<Metadata> GetMetadataFromAssemblyTypes([NotNull] Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (assembly.IsDynamic)
            {
                return ArrayExtensions<Metadata>.Empty();
            }

            FieldInfo[] publicConstantPrimitiveFields = assembly.GetTypes()
                .Where(type => type.IsClass && type.IsPublic && type.IsAbstract && type.IsSealed)
                .SelectMany(type => type.GetFields())
                .Where(
                    field =>
                        field.IsPublic &&
                        (field.FieldType == typeof(string)) &&
                        field.IsLiteral &&
                        !field.IsInitOnly)
                .ToArray();

            if (!publicConstantPrimitiveFields.Any())
            {
                return ArrayExtensions<Metadata>.Empty();
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
                return ArrayExtensions<Metadata>.Empty();
            }

            Metadata[] metadata = configurationMetadataFields
                .Select(
                    pair =>
                        new Metadata(pair.Field.GetRawConstantValue() as string ?? "INVALID_VALUE_NOT_A_STRING",
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
                            pair.Attribute.Tags))
                .ToArray();

            return metadata;
        }
    }
}
