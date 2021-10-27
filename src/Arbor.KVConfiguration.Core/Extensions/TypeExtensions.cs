using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions
{
    internal static class TypeExtensions
    {
        internal static bool IsPublicStaticClass([NotNull] this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsClass && (type.IsPublic || type.IsNestedPublic) && type.IsAbstract && type.IsSealed;
        }

        private static bool IsPublicClass([NotNull] this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsClass && (type.IsPublic || type.IsNestedPublic);
        }


        private static ImmutableArray<Type> GetLoadableTypes([NotNull] this Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            try
            {
                return assembly.GetTypes().ToImmutableArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(type => type is {}).ToImmutableArray()!;
            }
        }
        internal static ImmutableArray<FieldInfo> GetPublicConstantStringFields([NotNull] this Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var fields = assembly.GetLoadableTypes()
                .Where(IsPublicClass)
                .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.Static))
                .Where(field => field.IsPublicConstantStringField())
                .ToImmutableArray();

            return fields;
        }

        internal static ImmutableArray<FieldInfo> GetPublicConstantStringFields([NotNull] this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!IsPublicStaticClass(type))
            {
                return ImmutableArray<FieldInfo>.Empty;
            }

            var publicConstantStringFields = type.GetFields()
                .Where(field => field.IsPublicConstantStringField())
                .ToImmutableArray();

            return publicConstantStringFields;
        }

        private static bool IsPublicConstantStringField([NotNull] this FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
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