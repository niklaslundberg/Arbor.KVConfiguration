using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arbor.KVConfiguration.Core.Decorators;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Core.Metadata.Extensions;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public sealed class MultiSourceKeyValueConfiguration : IKeyValueConfigurationWithMetadata, IDisposable
    {
        private const string Arrow = "-->";
        private readonly AppSettingsDecoratorBuilder _appSettingsDecoratorBuilder;
        private readonly Action<string>? _logAction;
        private bool _isDisposed;
        private readonly string _sourceChain;

        public MultiSourceKeyValueConfiguration(
            [NotNull] AppSettingsDecoratorBuilder appSettingsDecoratorBuilder,
            Action<string>? logAction = null)
        {
            _appSettingsDecoratorBuilder = appSettingsDecoratorBuilder ??
                                           throw new ArgumentNullException(nameof(appSettingsDecoratorBuilder));

            _logAction = logAction;

            string decorators = BuildDecoratorsAsString(_appSettingsDecoratorBuilder);

            _sourceChain = "source chain: " + BuildChainAsString(_appSettingsDecoratorBuilder.AppSettingsBuilder) +
                          (string.IsNullOrWhiteSpace(decorators)
                              ? string.Empty
                              : ", decorators: " + decorators);
        }

        [PublicAPI]
        public string SourceChain
        {
            get
            {
                CheckIsDisposed();

                return _sourceChain;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            _appSettingsDecoratorBuilder.Dispose();
        }

        public ImmutableArray<string> AllKeys
        {
            get
            {
                CheckIsDisposed();

                return GetAllKeys(_appSettingsDecoratorBuilder.AppSettingsBuilder)
                    .Distinct(StringComparer.OrdinalIgnoreCase).ToImmutableArray();
            }
        }

        public ImmutableArray<StringPair> AllValues
        {
            get
            {
                CheckIsDisposed();

                IEnumerable<StringPair> stringPairs = AllKeys.Select(key => new StringPair(key,
                    GetValue(_appSettingsDecoratorBuilder.AppSettingsBuilder, key, _logAction).Item2));

                var immutableArray =
                    stringPairs.Select(pair => new StringPair(pair.Key,
                        DecorateValue(_appSettingsDecoratorBuilder, pair.Value))).ToImmutableArray();

                return immutableArray;
            }
        }

        private void CheckIsDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                CheckIsDisposed();
                ImmutableArray<MultipleValuesStringPair> values =
                    GetMultipleValues(_appSettingsDecoratorBuilder.AppSettingsBuilder, AllKeys);

                var multipleValuesStringPairs = values
                    .Select(item => new MultipleValuesStringPair(item.Key,
                        item.Values.Select(value => DecorateValue(_appSettingsDecoratorBuilder, value))
                            .ToImmutableArray()))
                    .ToImmutableArray();

                return multipleValuesStringPairs;
            }
        }

        public string this[string? key] => DecorateValue(_appSettingsDecoratorBuilder,
            GetValue(_appSettingsDecoratorBuilder.AppSettingsBuilder, key, _logAction).Item2);

        public ImmutableArray<KeyValueConfigurationItem> ConfigurationItems
        {
            get
            {
                CheckIsDisposed();

                return GetConfigurationItems(
                    _appSettingsDecoratorBuilder.AppSettingsBuilder).ToImmutableArray();
            }
        }

        private string BuildDecoratorsAsString(AppSettingsDecoratorBuilder? appSettingsDecoratorBuilder)
        {
            if (appSettingsDecoratorBuilder?.Decorator is null)
            {
                return string.Empty;
            }

            if (appSettingsDecoratorBuilder.Decorator is NullDecorator)
            {
                return string.Empty;
            }

            string result = appSettingsDecoratorBuilder.Decorator.ToString() ?? appSettingsDecoratorBuilder.Decorator.GetType().Name;

            if (appSettingsDecoratorBuilder.Previous is {})
            {
                result += Arrow + BuildDecoratorsAsString(appSettingsDecoratorBuilder.Previous);
            }

            return result;
        }

        private string BuildChainAsString(AppSettingsBuilder? builder)
        {
            if (builder?.KeyValueConfiguration is null)
            {
                return string.Empty;
            }

            string result = builder.KeyValueConfiguration.ToString() ?? builder.KeyValueConfiguration.GetType().Name;

            if (builder.Previous is {})
            {
                result += Arrow + BuildChainAsString(builder.Previous);
            }

            return result;
        }

        private static string[] GetAllKeys(AppSettingsBuilder? appSettingsBuilder)
        {
            if (appSettingsBuilder is null)
            {
                return Array.Empty<string>();
            }

            if (appSettingsBuilder.Previous is null)
            {
                return appSettingsBuilder.KeyValueConfiguration.AllKeys.ToArray();
            }

            string[] allPreviousKeys = GetAllKeys(appSettingsBuilder.Previous);

            string[] allKeys = appSettingsBuilder.KeyValueConfiguration.AllKeys.Concat(allPreviousKeys).ToArray();

            return allKeys;
        }

        private static string DecorateValue(AppSettingsDecoratorBuilder decorator, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            string? decorated = null;

            if (decorator.Previous is {})
            {
                decorated = DecorateValue(decorator.Previous, value);
            }

            return decorator.Decorator.GetValue(decorated ?? value);
        }

        private static (IKeyValueConfiguration, string) GetValue(
            AppSettingsBuilder? appSettingsBuilder,
            string? key,
            Action<string>? logAction)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new ValueTuple<IKeyValueConfiguration, string>(NoConfiguration.Empty, "");
            }

            if (appSettingsBuilder is null)
            {
                return new ValueTuple<IKeyValueConfiguration, string>(NoConfiguration.Empty, "");
            }

            string value = appSettingsBuilder.KeyValueConfiguration[key];

            if (string.IsNullOrWhiteSpace(value))
            {
                logAction?.Invoke(
                    $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} does not have a value for key '{key}'");

                return GetValue(appSettingsBuilder.Previous, key, logAction);
            }

            logAction?.Invoke(
                $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} has a value for key '{key}': '{value}'");

            (IKeyValueConfiguration, string) valueTuple = (appSettingsBuilder.KeyValueConfiguration, value);

            logAction?.Invoke($"For key '{key}', configuration source '{valueTuple.Item1.GetType().Name}' is used");

            return valueTuple;
        }

        private IKeyValueConfiguration GetConfiguratorDefining(AppSettingsBuilder? appSettingsBuilder, string key)
        {
            if (appSettingsBuilder is null)
            {
                return NoConfiguration.Empty;
            }

            if (appSettingsBuilder.KeyValueConfiguration.AllKeys.Contains(key, StringComparer.OrdinalIgnoreCase))
            {
                return appSettingsBuilder.KeyValueConfiguration;
            }

            return GetConfiguratorDefining(appSettingsBuilder.Previous, key);
        }

        private ImmutableArray<MultipleValuesStringPair> GetMultipleValues(
            AppSettingsBuilder? appSettingsBuilder,
            ImmutableArray<string> keysLeft)
        {
            if (appSettingsBuilder is null)
            {
                return ImmutableArray<MultipleValuesStringPair>.Empty;
            }

            var values =
                appSettingsBuilder.KeyValueConfiguration.AllWithMultipleValues
                    .Where(multipleValuesStringPair => keysLeft.Any(keyLeft =>
                        keyLeft.Equals(multipleValuesStringPair.Key, StringComparison.OrdinalIgnoreCase)))
                    .Where(multipleValuesStringPair => multipleValuesStringPair.HasNonEmptyValue)
                    .ToList();

            if (values.Count == 0)
            {
                _logAction?.Invoke(
                    $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} does not have any values for multiple values");

                return GetMultipleValues(appSettingsBuilder.Previous, keysLeft);
            }

            var keysLeftAfterValues = keysLeft.Except(values.Select(t => t.Key)).ToImmutableArray();

            if (keysLeftAfterValues.Any())
            {
                values.AddRange(GetMultipleValues(appSettingsBuilder.Previous, keysLeftAfterValues));
            }

            static string FormatValue(MultipleValuesStringPair pair)
            {
                return $"\'{pair.Key}\': [{string.Join("; ", pair.Values.Select(theValue => $"'{theValue}'"))}]";
            }

            string join = string.Join(", ", values.Select(FormatValue));

            _logAction?.Invoke(
                $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} has values: {join}");

            return values.ToImmutableArray();
        }

        private List<KeyValueConfigurationItem> GetConfigurationItems(AppSettingsBuilder appSettingsBuilder)
        {
            var configurationItems = new List<KeyValueConfigurationItem>();

            if (appSettingsBuilder.Previous is {})
            {
                configurationItems.AddRange(GetConfigurationItems(appSettingsBuilder.Previous));
            }

            if (appSettingsBuilder.KeyValueConfiguration is IKeyValueConfigurationWithMetadata
                keyValueConfigurationWithMetadata)
            {
                configurationItems.AddRange(keyValueConfigurationWithMetadata.ConfigurationItems);
            }
            else
            {
                configurationItems.AddRange(appSettingsBuilder.KeyValueConfiguration.GetKeyValueConfigurationItems());
            }

            return configurationItems;
        }

        [PublicAPI]
        public IKeyValueConfiguration? ConfiguratorFor(string? key, Action<string>? logAction = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            (IKeyValueConfiguration?, string) tuple =
                GetValue(_appSettingsDecoratorBuilder.AppSettingsBuilder, key, logAction);

            if (tuple.Item1 is NoConfiguration or null)
            {
                return GetConfiguratorDefining(_appSettingsDecoratorBuilder.AppSettingsBuilder, key!);
            }

            return tuple.Item1;
        }

        public override string ToString()
        {
            if (_isDisposed)
            {
                return nameof(MultiSourceKeyValueConfiguration) + "[DISPOSED]";
            }

            return $"{base.ToString()} [{SourceChain}]";
        }
    }
}