using System;
using System.Collections.Concurrent;

namespace Arbor.KVConfiguration.Urns
{
    public class ConfigurationInstanceHolder
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _configurationInstances =
            new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();

        public ImmutableArray<Type> RegisteredTypes => _configurationInstances.Keys.ToImmutableArray();

        public ImmutableDictionary<string, T?> GetInstances<T>() where T : class =>
            GetInstances(typeof(T)).Where(pair => pair.Value is T)
                .ToImmutableDictionary(pair => pair.Key, pair => pair.Value as T);

        public ImmutableDictionary<string, object> GetInstances(Type type)
        {
            if (!_configurationInstances.TryGetValue(type, out var instances))
            {
                return ImmutableDictionary<string, object>.Empty;
            }

            return instances.ToImmutableDictionary();
        }

        public bool TryGet<T>(string key, out T? instance) where T : class
        {
            var foundInstance = Get(typeof(T), key);

            if (foundInstance is T returnInstance)
            {
                instance = returnInstance;
                return true;
            }

            instance = default;
            return false;
        }

        public bool TryGet([NotNull] string key, [NotNull] Type type, out object? instance)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            var foundInstance = Get(type, key);

            if (foundInstance is null)
            {
                instance = default;
                return false;
            }

            if (type.IsInstanceOfType(foundInstance))
            {
                instance = foundInstance;
                return true;
            }

            instance = default;
            return false;
        }

        public object? Get(Type type, string key)
        {
            if (!_configurationInstances.TryGetValue(type, out var instances))
            {
                return default;
            }

            instances.TryGetValue(key, out var instance);

            return instance;
        }

        public bool TryRemove([NotNull] string key, [NotNull] Type type, out object? removed)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            if (_configurationInstances.TryGetValue(type, out var instances)
                && instances.TryRemove(key, out var removedItem))
            {
                removed = removedItem;
                return true;
            }

            removed = default;
            return false;
        }

        public void Add([NotNull] INamedInstance<object> instance)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (!_configurationInstances.TryGetValue(instance.Value.GetType(),
                out var typeInstanceDictionary))
            {
                var typeDictionary = new ConcurrentDictionary<string, object>();
                typeDictionary.AddOrUpdate(instance.Name, instance.Value, (name, found) => instance.Value);

                _configurationInstances.TryAdd(instance.Value.GetType(), typeDictionary);
            }
            else
            {
                typeInstanceDictionary.AddOrUpdate(instance.Name, instance.Value, (name, found) => instance.Value);
            }
        }
    }
}