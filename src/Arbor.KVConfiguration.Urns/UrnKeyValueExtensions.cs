using System;
using System.Collections.Generic;
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
        public static IReadOnlyCollection<T> GetInstances<T>(
            [NotNull] this IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            UrnAttribute urnAttribute = typeof(T).GetCustomAttribute<UrnAttribute>();

            if (urnAttribute == null)
            {
                throw new ArgumentException($"Found no {nameof(Urn).ToUpper()} for type {typeof(T)}");
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
                        urn.OriginalValue.StartsWith(urn.OriginalValue, StringComparison.InvariantCultureIgnoreCase))
                    .Where(key => key.NamespaceParts() == expectedParts)
                    .ToLookup(urn => urn.Parent, urn => urn).ToArray();

            List<T> items =
                instanceKeys.Select(keyValuePair => GetItem<T>(keyValueConfiguration, keyValuePair)).ToList();

            return items.AsReadOnly();
        }

        private static T GetItem<T>(IKeyValueConfiguration keyValueConfiguration, IGrouping<Urn, Urn> keyValuePair)
        {
            dynamic expando = new ExpandoObject();

            IDictionary<string, object> asDictionary = (IDictionary<string, object>)expando;

            foreach (Urn urn in keyValuePair)
            {
                string[] values =
                    keyValueConfiguration.AllWithMultipleValues
                        .Where(
                            multipleValuesStringPair =>
                            multipleValuesStringPair.Key.Equals(
                                urn.OriginalValue,
                                StringComparison.InvariantCultureIgnoreCase))
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

            T item = JsonConvert.DeserializeObject<T>(json);

            return item;
        }
    }
}