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

        public ImmutableArray<T> GetInstances<T>() where T : INamedInstance<T>
        {
            if (!_configurationInstances.TryGetValue(typeof(T), out ConcurrentDictionary<string, object> instances))
            {
                return ImmutableArray<T>.Empty;
            }

            return instances.Values.OfType<T>().ToImmutableArray();
        }

        public object Get(Type type, string key)
        {
            if (!_configurationInstances.TryGetValue(type, out ConcurrentDictionary<string, object> instances))
            {
                return ImmutableArray<object>.Empty;
            }

            instances.TryGetValue(key, out object instance);

            return instance;
        }

        public void Add([NotNull] INamedInstance<object> instance)
        {
            if (instance == null)
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
