using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Schema
{
    internal static class TypeExtensions
    {
        public static ImmutableArray<FieldInfo> GetPublicConstantStringFields([NotNull] this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            ImmutableArray<FieldInfo> fields = assembly.GetTypes()
                .Where(type => type.IsClass && type.IsPublic && type.IsAbstract && type.IsSealed)
                .SelectMany(type => type.GetFields())
                .Where(field => field.IsPublicConstantStringField()
                ).ToImmutableArray();

            return fields;
        }

        public static bool IsPublicConstantStringField([NotNull] this FieldInfo fieldInfo)
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
