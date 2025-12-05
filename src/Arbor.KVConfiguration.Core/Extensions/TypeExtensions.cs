using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Arbor.KVConfiguration.Core.Extensions;

internal static class TypeExtensions
{
    internal static bool IsPublicStaticClass(this Type type)
    {
        type.ThrowIfNull();

        return type.IsClass && (type.IsPublic || type.IsNestedPublic) && type.IsAbstract && type.IsSealed;
    }

    private static bool IsPublicClass(this Type type)
    {
        type.ThrowIfNull();

        return type.IsClass && (type.IsPublic || type.IsNestedPublic);
    }

    private static ImmutableArray<Type> GetLoadableTypes(this Assembly assembly)
    {
        assembly.ThrowIfNull();

        try
        {
            return [..assembly.GetTypes()];
        }
        catch (ReflectionTypeLoadException ex)
        {
            return [..ex.Types.Where(type => type is {})];
        }
    }
    internal static ImmutableArray<FieldInfo> GetPublicConstantStringFields(this Assembly assembly)
    {
        assembly.ThrowIfNull();

        var fields = assembly.GetLoadableTypes()
            .Where(IsPublicClass)
            .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(field => field.IsPublicConstantStringField())
            .ToImmutableArray();

        return fields;
    }

    internal static ImmutableArray<FieldInfo> GetPublicConstantStringFields(this Type type)
    {
        type.ThrowIfNull();

        if (!IsPublicStaticClass(type))
        {
            return ImmutableArray<FieldInfo>.Empty;
        }

        var publicConstantStringFields = type.GetFields()
            .Where(field => field.IsPublicConstantStringField())
            .ToImmutableArray();

        return publicConstantStringFields;
    }

    private static bool IsPublicConstantStringField(this FieldInfo fieldInfo)
    {
        fieldInfo.ThrowIfNull();

        return fieldInfo.IsPublic &&
               fieldInfo.FieldType == typeof(string) &&
               fieldInfo.IsLiteral &&
               !fieldInfo.IsInitOnly;
    }
}