using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

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
            if (!_configurationInstances.TryGetValue(type, out ConcurrentDictionary<string, object> instances))
            {
                return ImmutableDictionary<string, object>.Empty;
            }

            return instances.ToImmutableDictionary();
        }

        public bool TryGet<T>(string key, out T? instance) where T : class
        {
            object? foundInstance = Get(typeof(T), key);

            if (foundInstance is T returnInstance)
            {
                instance = returnInstance;
                return true;
            }

            instance = default;
            return false;
        }

        private object? Get(Type type, string key)
        {
            if (!_configurationInstances.TryGetValue(type, out ConcurrentDictionary<string, object> instances))
            {
                return default;
            }

            instances.TryGetValue(key, out object instance);

            return instance;
        }

        public void Add([NotNull] INamedInstance<object> instance)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (!_configurationInstances.TryGetValue(instance.Value.GetType(),
                out ConcurrentDictionary<string, object> typeInstanceDictionary))
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
