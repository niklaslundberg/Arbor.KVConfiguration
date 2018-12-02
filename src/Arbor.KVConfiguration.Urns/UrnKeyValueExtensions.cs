using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Arbor.KVConfiguration.Urns
{
    public static class UrnKeyValueExtensions
    {
        public static T GetInstance<T>(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration)
        {
            ImmutableArray<T> instances = GetInstances<T>(keyValueConfiguration);

            if (instances.Length > 1)
            {
                throw new InvalidOperationException($"Found multiple {typeof(T)}, expected 0 or 1");
            }

            if (!instances.Any())
            {
                return default;
            }

            return instances.Single();
        }

        public static ImmutableArray<INamedInstance<T>> GetNamedInstances<T>(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration)
        {
            return GetNamedInstances(keyValueConfiguration, typeof(T))
                .Select(item => item as INamedInstance<T>)
                .Where(item => item != null)
                .ToImmutableArray();
        }

        public static ImmutableArray<INamedInstance<object>> GetNamedInstances(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type)
        {
            ImmutableArray<(object, string, IDictionary<string, object>)> immutableArray =
                GetInstancesInternal(keyValueConfiguration, type);

            Type generic = typeof(NamedInstance<>);

            Type[] typeArgs = { type };

            Type constructed = generic.MakeGenericType(typeArgs);

            ImmutableArray<INamedInstance<object>> objects = immutableArray
                .Select(item => Activator.CreateInstance(constructed, item.Item1, item.Item2))
                .OfType<INamedInstance<object>>()
                .ToImmutableArray();

            return objects;
        }

        public static object GetInstance(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type)
        {
            object instance = GetInstance(keyValueConfiguration, type, null);

            return instance;
        }

        public static object GetInstance(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type,
            string instanceName)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            ImmutableArray<(object, string, IDictionary<string, object>)> instances =
                GetInstancesInternal(keyValueConfiguration, type);

            ImmutableArray<(object, string, IDictionary<string, object>)> filtered =
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

        public static ImmutableArray<T> GetInstances<T>(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration)
        {
            return GetInstances(keyValueConfiguration, typeof(T))
                .OfType<T>()
                .ToImmutableArray();
        }

        public static ImmutableArray<object> GetInstances(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type)
        {
            return GetInstancesInternal(keyValueConfiguration, type)
                .Select(item => item.Item1)
                .ToImmutableArray();
        }

        private static (object, string, IDictionary<string, object>) GetItem(
            IKeyValueConfiguration keyValueConfiguration,
            IGrouping<Urn, Urn> keyValuePair,
            Type type)
        {
            dynamic expando = new ExpandoObject();

#if DEBUG
            Console.WriteLine($"Creating type {type.FullName}, urn '{keyValuePair.Key}'");
#endif

            Urn instanceUri = keyValuePair.Key;

            string instanceName = instanceUri.Name;

            var asDictionary = (IDictionary<string, object>)expando;

            Urn[] allKeys = keyValueConfiguration.AllKeys
                .Select(key =>
                {
                    if (!Urn.TryParse(key, out Urn urn))
                    {
                        return null;
                    }

                    return urn;
                })
                .Where(urn => urn != null)
                .ToArray();

            Urn[] filteredKeys = allKeys
                .Where(urnKey =>
                    urnKey.IsInHierarchy(instanceUri))
                .Where(urnKey => urnKey.NamespaceParts() - instanceUri.NamespaceParts() == 1)
                .ToArray();

            foreach (Urn itemValue in filteredKeys)
            {
#if DEBUG
                Console.WriteLine($"Found key {itemValue}");
#endif

                string normalizedPropertyName = itemValue.Name.Replace("-", string.Empty);

                string[] values = keyValueConfiguration.AllWithMultipleValues
                    .Select(t =>
                    {
                        if (!Urn.TryParse(t.Key, out Urn urn))
                        {
                            return null;
                        }

                        return new { Urn = urn, Pair = t };
                    })
                    .Where(urn => urn != null)
                    .Where(urn => urn.Urn == itemValue)
                    .SelectMany(s => s.Pair.Values)
                    .ToArray();

                if (values.Length == 0)
                {
                    if (!asDictionary.ContainsKey(normalizedPropertyName))
                    {
                        asDictionary.Add(normalizedPropertyName, "");
                    }
#if DEBUG
                    Console.WriteLine("\tNo value");
#endif
                }
                else if (values.Length == 1)
                {
                    string value = values.Single();
                    asDictionary.Add(normalizedPropertyName, value);
#if DEBUG
                    Console.WriteLine($"\tSingle value '{normalizedPropertyName}': {value}");
#endif
                }
                else
                {
#if DEBUG
                    foreach (string value in values)
                    {
                        Console.WriteLine($"\tMultiple value: {value}");
                    }
#endif

                    asDictionary.Add(normalizedPropertyName, values.Select(value => value).ToArray());
                }
            }

            Urn[] subKeys = allKeys
                .Where(urnKey =>
                    urnKey.IsInHierarchy(instanceUri))
                .Where(urnKey => urnKey.NamespaceParts() - instanceUri.NamespaceParts() == 3)
                .ToArray();

            PropertyInfo[] typeProperties = type.GetProperties();

            foreach (IGrouping<Urn, Urn> subKeyGroup in subKeys.GroupBy(x => x.Parent))
            {
                PropertyInfo propertyInfo = typeProperties.SingleOrDefault(property => property.Name.Equals(subKeyGroup.Key.Parent.Name, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo != null)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        if (propertyInfo.PropertyType.IsGenericType)
                        {
                            Type propertyType = propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();

                            if (propertyType != null)
                            {
                                (object, string, IDictionary<string, object>) subItem = GetItem(keyValueConfiguration,
                                    subKeyGroup,
                                    propertyType);

                                Console.WriteLine($"Found sub item" + subItem.Item1);

                                if (!asDictionary.ContainsKey(subKeyGroup.Key.Parent.Name))
                                {
                                    var list = new List<object>();
                                    asDictionary.Add(new KeyValuePair<string, object>(subKeyGroup.Key.Parent.Name, list));
                                }

                                if (asDictionary[subKeyGroup.Key.Parent.Name] is List<object> childList)
                                {
                                    childList.Add(subItem.Item1);
                                }
                            }
                        }
                    }
                }
            }

            Urn[] subProperties = allKeys
                .Where(urnKey =>
                    urnKey.IsInHierarchy(instanceUri))
                .Where(urnKey => urnKey.NamespaceParts() - instanceUri.NamespaceParts() == 2)
                .ToArray();

            IEnumerable<IGrouping<Urn, Urn>> subGroups = subProperties.GroupBy(x => x.Parent);

            foreach (IGrouping<Urn, Urn> subProperty in subGroups)
            {
                PropertyInfo subPropertyInfo = typeProperties.SingleOrDefault(property => property.Name.Equals(subProperty.Key.Name, StringComparison.OrdinalIgnoreCase));

                if (subPropertyInfo != null)
                {
                    (object, string, IDictionary<string, object>) subPropertyInstance = GetItem(keyValueConfiguration,
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

            object item;

            try
            {
                JsonConverter[] converters = { new StringValuesJsonConverter() };
                item = JsonConvert.DeserializeObject(json, type, converters);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                throw new InvalidOperationException($"Could not deserialize json '{json}' to target type {type.FullName}", ex);
            }
            catch (Exception ex)
            {
                ImmutableArray<(string, string)> errorProperties = type
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(property =>
                    {
                        string matchingKey = asDictionary.Keys.SingleOrDefault(key =>
                            key.Equals(property.Name, StringComparison.OrdinalIgnoreCase));

                        if (matchingKey is null)
                        {
                            return ("", null);
                        }

                        if (!asDictionary.TryGetValue(matchingKey, out object value))
                        {
                            return ("", null);
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

                        return (property.Name, null);
                    })
                    .Where(tuple => tuple.Item2 != null)
                    .ToImmutableArray();

                string specifiedErrors = string.Join(", ", errorProperties.Select(ep => ep.Item2));

                throw new InvalidOperationException(
                    $"{specifiedErrors} Could not deserialize json '{json}' to target type {type.FullName}".Trim(),
                    ex);
            }

            return (item, instanceName, asDictionary);
        }

        private static ImmutableArray<(object, string, IDictionary<string, object>)> GetInstancesInternal(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration,
            [NotNull] Type type)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var urnAttribute = type.GetCustomAttribute<UrnAttribute>();

            if (urnAttribute == null)
            {
                throw new ArgumentException($"Found no {nameof(Urn).ToUpper(CultureInfo.InvariantCulture)} for type {type}");
            }

            Urn typeUrn = urnAttribute.Urn;

            int parts = typeUrn.NamespaceParts();

            int expectedParts = parts + 2;

            IGrouping<Urn, Urn>[] instanceKeys =
                keyValueConfiguration.AllKeys
                    .Where(key => key.IsUrn())
                    .Select(key => new Urn(key))
                    .Where(
                        urn =>
                            urn.IsInHierarchy(typeUrn))
                    .Where(key => key.NamespaceParts() == expectedParts)
                    .ToLookup(urn => urn.Parent, urn => urn).ToArray();

            ImmutableArray<(object, string, IDictionary<string, object>)> items = instanceKeys
                .OrderBy(key => key.Key.ToString())
                .Select(keyValuePair => GetItem(keyValueConfiguration, keyValuePair, type))
                .Where(instance => instance.Item1 != null)
                .ToImmutableArray();

            return items;
        }
    }
}
