using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Arbor.KVConfiguration.Core;
using Arbor.Primitives;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Urns
{
    public static class UrnKeyValueExtensions
    {
        [PublicAPI]
        public static ImmutableArray<INamedInstance<T>> GetNamedInstances<T>(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration) =>
            GetNamedInstances(keyValueConfiguration, typeof(T))
                .Select(item => item as INamedInstance<T>)
                .Where(item => item is { })
                .ToImmutableArray()!;

        [PublicAPI]
        public static ImmutableArray<INamedInstance<object>> GetNamedInstances(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type)
        {
            ImmutableArray<(object?, string, IDictionary<string, object?>)> immutableArray =
                GetInstancesInternal(keyValueConfiguration, type);

            var generic = typeof(NamedInstance<>);

            Type[] typeArgs = {type};

            var constructedGenericType = generic.MakeGenericType(typeArgs);

            var objects = immutableArray
                .Select(item => Activator.CreateInstance(constructedGenericType, item.Item1, item.Item2))
                .OfType<INamedInstance<object>>()
                .ToImmutableArray();

            return objects;
        }

        [PublicAPI]
        public static object? GetInstance(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type) => GetInstance(keyValueConfiguration, type, null);

        [PublicAPI]
        public static object? GetInstance(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type,
            string? instanceName)
        {
            if (keyValueConfiguration is null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            ImmutableArray<(object?, string, IDictionary<string, object?>)> instances =
                GetInstancesInternal(keyValueConfiguration, type);

            ImmutableArray<(object?, string, IDictionary<string, object?>)> filtered =
                string.IsNullOrWhiteSpace(instanceName)
                    ? instances
                    : instances
                        .Where(instance => instance.Item2.Equals(instanceName, StringComparison.OrdinalIgnoreCase))
                        .ToImmutableArray();

            if (filtered.Length > 1)
            {
                IEnumerable<string> keys = filtered.Select(instance => instance.Item2);
                throw new InvalidOperationException($"Found multiple {type}, expected 0 or 1, instance keys: {keys}");
            }

            if (filtered.Length == 0)
            {
                return default;
            }

            return filtered.Single().Item1;
        }

        public static T? GetInstance<T>(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration)
        {
            ImmutableArray<T> instances = GetInstances<T>(keyValueConfiguration);

            if (instances.Length > 1)
            {
                throw new InvalidOperationException(
                    $"Found multiple instances of type {typeof(T).FullName}, expected 0 or 1");
            }

            if (!instances.Any())
            {
                return default;
            }

            return instances.Single();
        }
        public static ImmutableArray<T> GetInstances<T>(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration) =>
            GetInstances(keyValueConfiguration, typeof(T))
                .OfType<T>()
                .ToImmutableArray();

        [PublicAPI]
        public static ImmutableArray<object?> GetInstances(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type) =>
            GetInstancesInternal(keyValueConfiguration, type)
                .Select(item => item.Item1)
                .ToImmutableArray();

        private static (object?, string, IDictionary<string, object?>) GetItem(
            IKeyValueConfiguration keyValueConfiguration,
            IGrouping<Urn, Urn> keyValuePair,
            Type type)
        {
            dynamic expando = new ExpandoObject();

#if DEBUG
            Debug.WriteLine($"Creating type {type.FullName}, urn '{keyValuePair.Key}'");
#endif

            var instanceUri = keyValuePair.Key;

            string? instanceName = instanceUri.Name;

            var asDictionary = (IDictionary<string, object?>)expando;

            Urn[] allKeys = keyValueConfiguration.AllKeys
                .Select(key =>
                {
                    if (!Urn.TryParse(key, out var urn))
                    {
                        return null;
                    }

                    return urn;
                })
                .Where(urn => urn.HasValue)
                .Select(urn => urn!.Value)
                .ToArray()!;

            var filteredKeys = allKeys.Where(urnKey =>
                urnKey.Name is { } &&
                urnKey.IsInHierarchy(instanceUri) &&
                urnKey.NamespaceParts() - instanceUri.NamespaceParts() == 1).ToArray();

            foreach (var itemValue in filteredKeys)
            {
#if DEBUG
                Debug.WriteLine($"Found key {itemValue}");
#endif

                string normalizedPropertyName = itemValue.Name!.Replace("-", string.Empty);

                string[] values = keyValueConfiguration.AllWithMultipleValues
                    .Select(t =>
                    {
                        if (!Urn.TryParse(t.Key, out var urn))
                        {
                            return null;
                        }

                        return new {Urn = urn, Pair = t};
                    })
                    .Where(urn => urn is {} && urn.Urn is { } && urn.Urn == itemValue)
                    .SelectMany(s => s!.Pair.Values)
                    .ToArray();

                if (values.Length == 0)
                {
                    if (!asDictionary.ContainsKey(normalizedPropertyName))
                    {
                        asDictionary.Add(normalizedPropertyName, "");
                    }
#if DEBUG
                    Debug.WriteLine("\tNo value");
#endif
                }
                else if (values.Length == 1)
                {
                    string value = values.Single();
                    asDictionary.Add(normalizedPropertyName, value);
#if DEBUG
                    Debug.WriteLine($"\tSingle value '{normalizedPropertyName}': {value}");
#endif
                }
                else
                {
#if DEBUG
                    foreach (string value in values)
                    {
                        Debug.WriteLine($"\tMultiple value: {value}");
                    }
#endif

                    asDictionary.Add(normalizedPropertyName, values.Select(value => value).ToArray());
                }
            }

            var subKeys = allKeys
                .Where(urnKey =>
                    urnKey.IsInHierarchy(instanceUri) && urnKey.NamespaceParts() - instanceUri.NamespaceParts() == 3)
                .ToArray();

            var typeProperties = type.GetProperties();

            foreach (var subKeyGroup in subKeys.GroupBy(x => x.Parent))
            {
                var propertyInfo = typeProperties.SingleOrDefault(property =>
                    property.Name.Equals(subKeyGroup.Key.Parent.Name, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo is {}
                    && typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType)
                    && propertyInfo.PropertyType.IsGenericType)
                {
                    var propertyType = propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();

                    if (propertyType is { })
                    {
                        var subItem = GetItem(keyValueConfiguration,
                            subKeyGroup,
                            propertyType);

                        string? name = subKeyGroup.Key.Parent.Name;

                        if (name is null)
                        {
                            continue;
                        }

                        if (!asDictionary.ContainsKey(name))
                        {
                            var list = new List<object>();

                            asDictionary.Add(
                                new KeyValuePair<string, object?>(name, list));
                        }

                        if (asDictionary[name] is List<object> childList && subItem.Item1 is {})
                        {
                            childList.Add(subItem.Item1);
                        }
                    }
                }
            }

            var subProperties = allKeys
                .Where(urnKey =>
                    urnKey.IsInHierarchy(instanceUri) && urnKey.NamespaceParts() - instanceUri.NamespaceParts() == 2)
                .ToArray();

            var subGroups = subProperties.GroupBy(x => x.Parent);

            foreach (var subProperty in subGroups)
            {
                var subPropertyInfo = typeProperties.SingleOrDefault(property =>
                    property.Name.Equals(subProperty.Key.Name, StringComparison.OrdinalIgnoreCase));

                if (subPropertyInfo is { })
                {
                    var subPropertyInstance = GetItem(keyValueConfiguration,
                        subProperty,
                        subPropertyInfo.PropertyType);

                    asDictionary[subPropertyInfo.Name] = subPropertyInstance.Item1;
                }
            }

            if (asDictionary.Keys.Count == 0)
            {
                return default;
            }

            if (asDictionary.Values.All(value =>
                value is null || (value is string text && string.IsNullOrWhiteSpace(text))))
            {
                return default;
            }

            string json = JsonConvert.SerializeObject(expando);

            object? item;

            try
            {
                JsonConverter[] converters = {new StringValuesJsonConverter()};
                item = JsonConvert.DeserializeObject(json, type, converters);
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException(
                    $"Could not deserialize json '{json}' to target type {type.FullName}", ex);
            }
            catch (Exception ex)
            {
                var invalidOperationException = CreateException(type, asDictionary, json, ex);

                throw invalidOperationException;
            }

            return (item, instanceName ?? "", asDictionary);
        }

        private static InvalidOperationException CreateException(Type type,
            IDictionary<string, object?> asDictionary,
            string json,
            Exception ex)
        {
            ImmutableArray<(string, string?)> errorProperties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(property =>
                {
                    string? matchingKey = asDictionary.Keys.SingleOrDefault(key =>
                        key.Equals(property.Name, StringComparison.OrdinalIgnoreCase));

                    if (matchingKey is null)
                    {
                        return ("", null)!;
                    }

                    if (!asDictionary.TryGetValue(matchingKey, out object? value))
                    {
                        return ("", null)!;
                    }

                    if ((property.PropertyType == typeof(string)
                         || !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                        && value is IEnumerable enumerable)
                    {
                        object[] objects = enumerable.OfType<object>().ToArray();

                        if (objects.Length > 1)
                        {
                            if (value is string stringValue)
                            {
                                return (property.Name,
                                    $"The property of type {property.PropertyType.Name} with name '{property.Name}' has multiple values {stringValue}");
                            }

                            return (property.Name,
                                    $"The property of type {property.PropertyType.Name} with name '{property.Name}' has multiple values {string.Join(", ", objects.Select(o => $"'{o}'"))}"
                                );
                        }
                    }

                    return (property.Name, null)!;
                })
                .Where(tuple => tuple.Item2 is { })
                .ToImmutableArray()!;

            string specifiedErrors = string.Join(", ", errorProperties.Select(ep => ep.Item2));

            return new InvalidOperationException(
                $"{specifiedErrors} Could not deserialize json '{json}' to target type {type.FullName}".Trim(),
                ex);
        }

        private static ImmutableArray<(object?, string, IDictionary<string, object?>)> GetInstancesInternal(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type)
        {
            if (keyValueConfiguration is null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var urnAttribute = type.GetCustomAttribute<UrnAttribute>() ?? throw new ArgumentException(
                    $"Could not get instance of type {type.FullName}. Found no {nameof(Urn).ToUpper(CultureInfo.InvariantCulture)}. Expected class attribute {typeof(UrnAttribute).FullName}");

            var typeUrn = urnAttribute.Urn;

            if (typeUrn is null)
            {
                return ImmutableArray<(object?, string, IDictionary<string, object?>)>.Empty;
            }

            int parts = typeUrn.Value.NamespaceParts();

            int expectedParts = parts + 2;

            IGrouping<Urn, Urn>[] instanceKeys =
                keyValueConfiguration.AllKeys
                    .Where(key => key.IsUrn())
                    .Select(Urn.Parse)
                    .Where(
                        urn =>
                            urn.IsInHierarchy(typeUrn.Value))
                    .Where(key => key.NamespaceParts() == expectedParts)
                    .ToLookup(urn => urn.Parent, urn => urn).ToArray();

            var items = instanceKeys
                .OrderBy(key => key.Key.ToString())
                .Select(keyValuePair => GetItem(keyValueConfiguration, keyValuePair, type))
                .Where(instance => instance.Item1 is { })
                .ToImmutableArray();

            return items;
        }
    }
}