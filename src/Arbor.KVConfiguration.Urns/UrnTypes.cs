using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Arbor.KVConfiguration.Urns
{
    public static class UrnTypes
    {
        public static ImmutableArray<UrnTypeMapping> GetUrnTypesInAssemblies(params Assembly[] assemblies)
        {
            UrnTypeMapping? HasUrnAttribute(Type type)
            {
                var customAttribute = type.GetCustomAttribute<UrnAttribute>();

                return customAttribute?.Urn is null ? null : new UrnTypeMapping(type, customAttribute.Urn);
            }

            ImmutableArray<UrnTypeMapping> urnMappedTypes = assemblies
                .SelectMany(assembly =>
                    assembly.ExportedTypes
                        .Where(type => !type.IsAbstract && type.IsPublic)
                        .Select(HasUrnAttribute))
                .Where(mapping => mapping is object)
                .ToImmutableArray()!;

            return urnMappedTypes;
        }
    }
}