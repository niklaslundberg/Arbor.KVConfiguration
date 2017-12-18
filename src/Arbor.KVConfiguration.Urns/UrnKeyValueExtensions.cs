using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;
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

        public static object GetInstance(
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

            ImmutableArray<object> instances = GetInstances(keyValueConfiguration, type);

            if (instances.Length > 1)
            {
                throw new InvalidOperationException($"Found multiple {type}, expected 0 or 1");
            }

            if (!instances.Any())
            {
                return default;
            }

            return instances.Single();
        }

        public static ImmutableArray<T> GetInstances<T>(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration)
        {
            return GetInstances(keyValueConfiguration, typeof(T)).OfType<T>().ToImmutableArray();
        }

        public static ImmutableArray<object> GetInstances(
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
                throw new ArgumentException($"Found no {nameof(Urn).ToUpper()} for type {type}");
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

            ImmutableArray<object> items = instanceKeys
                .OrderBy(key => key.Key.ToString())
                .Select(keyValuePair => GetItem(keyValueConfiguration, keyValuePair, type))
                .Where(instance => instance != null)
                .ToImmutableArray();

            return items;
        }

        private static object GetItem(
            IKeyValueConfiguration keyValueConfiguration,
            IGrouping<Urn, Urn> keyValuePair,
            Type type)
        {
            dynamic expando = new ExpandoObject();

            Console.WriteLine($"Creating type {type.FullName}, urn {keyValuePair.Key}");

            Urn instanceUri = keyValuePair.Key;

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
                .ToArray();

            foreach (Urn itemValue in filteredKeys)
            {
                Console.WriteLine($"Found key {itemValue}");

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

                    Console.WriteLine("\tNo value");
                }
                else if (values.Length == 1)
                {
                    string value = values.Single();
                    asDictionary.Add(normalizedPropertyName, value);

                    Console.WriteLine($"\tSingle value: {value}");
                }
                else
                {
                    foreach (string value in values)
                    {
                        Console.WriteLine($"\tMultiple value: {value}");
                    }

                    asDictionary.Add(normalizedPropertyName, values.Select(s => s).ToArray());
                }
            }

            if (asDictionary.Keys.Count == 0)
            {
                return null;
            }

            if (asDictionary.Values.All(value => value is null || (value is string text && string.IsNullOrWhiteSpace(text))))
            {
                return null;
            }

            string json = JsonConvert.SerializeObject(expando);

            object item;

            try
            {
                JsonConverter[] converters = { new StringValuesJsonConverter()};
                item = JsonConvert.DeserializeObject(json, type, converters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Could not deserialize json '{json}' to target type {type.FullName}",
                    ex);
            }

            return item;
        }
    }
}
