using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using Arbor.KVConfiguration.Core.Extensions;

namespace Arbor.KVConfiguration.Core;

public sealed class InMemoryKeyValueConfiguration : IKeyValueConfiguration, IDisposable
{
    private readonly string _name;
    private ImmutableArray<string> _allKeys;
    private bool _disposed;
    private Dictionary<string, ImmutableArray<string>>? _keyValueDictionary;

    public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection) : this(nameValueCollection,
        string.Empty)
    {
    }

    public InMemoryKeyValueConfiguration(NameValueCollection nameValueCollection, string? name)
    {
        nameValueCollection.ThrowIfNull();

        _name = name ?? string.Empty;

        _keyValueDictionary =
            new Dictionary<string, ImmutableArray<string>>(nameValueCollection.Count + 1,
                StringComparer.OrdinalIgnoreCase);

        var keys = nameValueCollection.AllKeys
            .Where(key => key is {})
            .Cast<string>()
            .ToImmutableArray();

        foreach (string key in keys)
        {
            ImmutableArray<string> values = nameValueCollection.GetValues(key!).SafeToImmutableArray();

            if (!string.IsNullOrWhiteSpace(key))
            {
                if (!_keyValueDictionary.TryGetValue(key, out ImmutableArray<string> value))
                {
                    _keyValueDictionary.Add(key, values);
                }
                else
                {
                    _keyValueDictionary[key] = value.AddRange(values);
                }
            }
        }

        _allKeys = [.._keyValueDictionary.Keys];
    }

    public ImmutableArray<string> AllKeys
    {
        get
        {
            CheckDisposed();

            return _allKeys;
        }
    }

    public ImmutableArray<StringPair> AllValues
    {
        get
        {
            CheckDisposed();

            return [..AllKeys.Select(key => new StringPair(key, GetCombinedValues(key)))];
        }
    }

    public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
    {
        get
        {
            CheckDisposed();

            return [
                ..AllKeys
                    .Select(key => new MultipleValuesStringPair(key, _keyValueDictionary![key]))
            ];
        }
    }

    public string this[string? key] => GetCombinedValues(key);

    private string GetCombinedValues(string? key)
    {
        CheckDisposed();

        if (key is null)
        {
            return string.Empty;
        }

        if (!_keyValueDictionary!.TryGetValue(key, out ImmutableArray<string> values))
        {
            return string.Empty;
        }

        if (values.IsEmpty)
        {
            return string.Empty;
        }

        if (values.Length == 1)
        {
            return values[0];
        }

        return string.Join(",", values);
    }

    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(ToString());
        }
    }

    public override string ToString()
    {
        if (!string.IsNullOrWhiteSpace(_name))
        {
            return $"{base.ToString()} [name: '{_name}']";
        }

        return $"{base.ToString()} [name: 'unnamed']";
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _keyValueDictionary?.Clear();
            _keyValueDictionary = null;
            _allKeys = default;
            _disposed = true;
        }
    }
}