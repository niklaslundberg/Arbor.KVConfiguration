using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
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
                            urn.OriginalValue.StartsWith(urn.OriginalValue, StringComparison.OrdinalIgnoreCase))
                    .Where(key => key.NamespaceParts() == expectedParts)
                    .ToLookup(urn => urn.Parent, urn => urn).ToArray();

            ImmutableArray<object> items = instanceKeys
                .Select(keyValuePair => GetItem(keyValueConfiguration, keyValuePair, type))
                .ToImmutableArray();

            return items;
        }

        private static object GetItem(IKeyValueConfiguration keyValueConfiguration, IGrouping<Urn, Urn> keyValuePair, Type type)
        {
            dynamic expando = new ExpandoObject();

            var asDictionary = (IDictionary<string, object>)expando;

            foreach (Urn urn in keyValuePair)
            {
                string[] values =
                    keyValueConfiguration.AllWithMultipleValues
                        .Where(
                            multipleValuesStringPair =>
                                multipleValuesStringPair.Key.Equals(
                                    urn.OriginalValue,
                                    StringComparison.OrdinalIgnoreCase))
                        .SelectMany(s => s.Values)
                        .ToArray();

                string normalizedPropertyName = urn.Name.Replace("-", string.Empty);

                if (values.Length == 1)
                {
                    string singleValue = values.Single();
                    asDictionary.Add(normalizedPropertyName, singleValue);
                }
                else
                {
                    asDictionary.Add(normalizedPropertyName, values);
                }
            }

            string json = JsonConvert.SerializeObject(expando);

            object item = JsonConvert.DeserializeObject(json, type);

            return item;
        }
    }
}
