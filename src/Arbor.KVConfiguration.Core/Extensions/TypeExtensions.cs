using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions
{
    internal static class TypeExtensions
    {
        internal static bool IsPublicStaticClass([NotNull] this TypeInfo type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsClass && (type.IsPublic || type.IsNestedPublic) && type.IsAbstract && type.IsSealed;
        }

        internal static ImmutableArray<FieldInfo> GetPublicConstantStringFields([NotNull] this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            ImmutableArray<FieldInfo> fields = assembly.DefinedTypes
                .Where(IsPublicStaticClass)
                .SelectMany(type => type.DeclaredFields)
                .Where(field => field.IsPublicConstantStringField())
                .ToImmutableArray();

            return fields;
        }

        internal static ImmutableArray<FieldInfo> GetPublicConstantStringFields([NotNull] this TypeInfo type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!(IsPublicStaticClass(type)))
            {
                return ImmutableArray<FieldInfo>.Empty;
            }

            ImmutableArray<FieldInfo> publicConstantStringFields = type.DeclaredFields
                .Where(field => field.IsPublicConstantStringField())
                .ToImmutableArray();

            return publicConstantStringFields;
        }

        internal static bool IsPublicConstantStringField([NotNull] this FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException(nameof(fieldInfo));
            }

            return fieldInfo.IsPublic &&
                   fieldInfo.FieldType == typeof(string) &&
                   fieldInfo.IsLiteral &&
                   !fieldInfo.IsInitOnly;
        }
    }
}
