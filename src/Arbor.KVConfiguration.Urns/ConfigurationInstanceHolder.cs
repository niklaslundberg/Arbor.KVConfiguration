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

        public ImmutableArray<Type> RegisteredNamedInstanceTypes => _configurationInstances
            .SelectMany(s => s.Value.Values.Select(v => v.GetType())).ToImmutableArray();

        public ImmutableDictionary<string, object> GetInstances(Type type)
        {
            if (!_configurationInstances.TryGetValue(type, out ConcurrentDictionary<string, object> instances))
            {
                return ImmutableDictionary<string, object>.Empty;
            }

            return instances.ToImmutableDictionary();
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
