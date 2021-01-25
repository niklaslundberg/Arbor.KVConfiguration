using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Arbor.KVConfiguration.Urns
{
    public static class UrnTypes
    {
        public static ImmutableArray<UrnTypeMapping> GetUrnTypesInAssemblies(Action<Exception>? exceptionHandler, params Assembly[] assemblies)
        {
            IEnumerable<UrnTypeMapping> TryGetTypes(Assembly assembly)
            {
                try
                {
                    return assembly.ExportedTypes
                        .Where(type => !type.IsAbstract && type.IsPublic)
                        .Select(HasUrnAttribute)
                        .Where(item => item is not null)!;
                }
                catch (FileLoadException ex)
                {
                    exceptionHandler?.Invoke(ex);
                }
                catch (FileNotFoundException ex)
                {
                    exceptionHandler?.Invoke(ex);
                }
                catch (TypeLoadException ex)
                {
                    exceptionHandler?.Invoke(ex);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exceptionHandler?.Invoke(ex);
                }

                return ImmutableArray<UrnTypeMapping>.Empty;
            }

            UrnTypeMapping? HasUrnAttribute(Type type)
            {
                var customAttribute = type.GetCustomAttribute<UrnAttribute>();

                return customAttribute?.Urn is null
                    ? null
                    : new UrnTypeMapping(type, customAttribute.Urn.Value);
            }

            ImmutableArray<UrnTypeMapping> urnMappedTypes = assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(TryGetTypes)
                .Where(mapping => mapping is not null)
                .ToImmutableArray()!;

            return urnMappedTypes;
        }
    }
}