using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.KVConfiguration.Urns;

public class ConfigurationInstanceHolder
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _configurationInstances = new();

    public ImmutableArray<Type> RegisteredTypes => [.._configurationInstances.Keys];

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
        object? foundInstance = Get(typeof(T), key);

        if (foundInstance is T returnInstance)
        {
            instance = returnInstance;
            return true;
        }

        instance = null;
        return false;
    }

    public bool TryGet(string key, Type type, out object? instance)
    {
        type.ThrowIfNull(nameof(type));

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        }

        object? foundInstance = Get(type, key);

        if (foundInstance is null)
        {
            instance = null;
            return false;
        }

        if (type.IsInstanceOfType(foundInstance))
        {
            instance = foundInstance;
            return true;
        }

        instance = null;
        return false;
    }

    public object? Get(Type type, string key)
    {
        if (!_configurationInstances.TryGetValue(type, out var instances))
        {
            return null;
        }

        instances.TryGetValue(key, out object? instance);

        return instance;
    }

    public bool TryRemove(string key, Type type, out object? removed)
    {
        type.ThrowIfNull(nameof(type));

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        }

        if (_configurationInstances.TryGetValue(type, out var instances)
            && instances.TryRemove(key, out object? removedItem))
        {
            removed = removedItem;
            return true;
        }

        removed = null;
        return false;
    }

    public void Add(INamedInstance<object> instance)
    {
        instance.ThrowIfNull(nameof(instance));

        if (!_configurationInstances.TryGetValue(instance.Value.GetType(),
                out var typeInstanceDictionary))
        {
            var typeDictionary = new ConcurrentDictionary<string, object>();
            typeDictionary.AddOrUpdate(instance.Name, instance.Value, (_, _) => instance.Value);

            _configurationInstances.TryAdd(instance.Value.GetType(), typeDictionary);
        }
        else
        {
            typeInstanceDictionary.AddOrUpdate(instance.Name, instance.Value, (_, _) => instance.Value);
        }
    }
}