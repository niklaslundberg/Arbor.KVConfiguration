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
    public class MultiSourceKeyValueConfiguration : IKeyValueConfigurationWithMetadata
    {
        private readonly AppSettingsDecoratorBuilder _appSettingsDecoratorBuilder;
        private readonly Action<string> _logAction;

        private const string Arrow = "-->";

        public MultiSourceKeyValueConfiguration(
            [NotNull] AppSettingsDecoratorBuilder appSettingsDecoratorBuilder,
            Action<string> logAction = null)
        {
            _appSettingsDecoratorBuilder = appSettingsDecoratorBuilder ??
                                           throw new ArgumentNullException(nameof(appSettingsDecoratorBuilder));
            _logAction = logAction;

            string decorators = BuildDecoratorsAsString(_appSettingsDecoratorBuilder);

            SourceChain = "source chain: " + BuildChainAsString(_appSettingsDecoratorBuilder.AppSettingsBuilder) + (string.IsNullOrWhiteSpace(decorators) ? string.Empty : ", decorators: " + decorators);
        }

        private string BuildDecoratorsAsString(AppSettingsDecoratorBuilder appSettingsDecoratorBuilder)
        {
            if (appSettingsDecoratorBuilder is null || appSettingsDecoratorBuilder.Decorator is null)
            {
                return string.Empty;
            }

            if (appSettingsDecoratorBuilder.Decorator is NullDecorator)
            {
                return string.Empty;
            }

            string result = appSettingsDecoratorBuilder.Decorator.ToString();

            if (appSettingsDecoratorBuilder.Previous != null)
            {
                result += Arrow + BuildDecoratorsAsString(appSettingsDecoratorBuilder.Previous);
            }

            return result;
        }

        private string BuildChainAsString(AppSettingsBuilder builder)
        {
            if (builder is null || builder.KeyValueConfiguration is null)
            {
                return string.Empty;
            }

            string result = builder.KeyValueConfiguration.ToString();

            if (builder.Previous != null)
            {
                result += Arrow + BuildChainAsString(builder.Previous);
            }

            return result;
        }

        public ImmutableArray<string> AllKeys => GetAllKeys(_appSettingsDecoratorBuilder.AppSettingsBuilder)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToImmutableArray();

        public ImmutableArray<StringPair> AllValues
        {
            get
            {
                IEnumerable<StringPair> stringPairs = AllKeys.Select(key => new StringPair(key,
                    GetValue(_appSettingsDecoratorBuilder.AppSettingsBuilder, key, _logAction).Item2));
                ImmutableArray<StringPair> immutableArray =
                    stringPairs.Select(pair => new StringPair(pair.Key,
                        DecorateValue(_appSettingsDecoratorBuilder, pair.Value))).ToImmutableArray();

                return immutableArray;
            }
        }

        public ImmutableArray<MultipleValuesStringPair> AllWithMultipleValues
        {
            get
            {
                ImmutableArray<MultipleValuesStringPair> values =
                    GetMultipleValues(_appSettingsDecoratorBuilder.AppSettingsBuilder, AllKeys);

                ImmutableArray<MultipleValuesStringPair> multipleValuesStringPairs = values
                    .Select(item => new MultipleValuesStringPair(item.Key,
                        item.Values.Select(value => DecorateValue(_appSettingsDecoratorBuilder, value))
                            .ToImmutableArray()))
                    .ToImmutableArray();

                return multipleValuesStringPairs;
            }
        }

        public string this[string key] => DecorateValue(_appSettingsDecoratorBuilder,
            GetValue(_appSettingsDecoratorBuilder.AppSettingsBuilder, key, _logAction).Item2);

        public ImmutableArray<KeyValueConfigurationItem> ConfigurationItems => GetConfigurationItems(
            _appSettingsDecoratorBuilder.AppSettingsBuilder).ToImmutableArray();

        public string SourceChain { get; }

        public IKeyValueConfiguration ConfiguratorFor(string key, Action<string> logAction = null)
        {
            (IKeyValueConfiguration, string) tuple =
                GetValue(_appSettingsDecoratorBuilder.AppSettingsBuilder, key, logAction);

            if (tuple.Item1 is NoConfiguration || tuple.Item1 is null)
            {
                return GetConfiguratorDefining(_appSettingsDecoratorBuilder.AppSettingsBuilder, key);
            }

            return tuple.Item1;
        }

        private static string[] GetAllKeys(AppSettingsBuilder appSettingsBuilder)
        {
            if (appSettingsBuilder == null)
            {
                return Array.Empty<string>();
            }

            if (appSettingsBuilder.Previous == null)
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

            string decorated = null;
            if (decorator.Previous != null)
            {
                decorated = DecorateValue(decorator.Previous, value);
            }

            return decorator.Decorator.GetValue(decorated ?? value);
        }

        private static (IKeyValueConfiguration, string) GetValue(
            AppSettingsBuilder appSettingsBuilder,
            string key,
            Action<string> logAction)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new ValueTuple<IKeyValueConfiguration, string>(new NoConfiguration(), "");
            }

            if (appSettingsBuilder == null)
            {
                return new ValueTuple<IKeyValueConfiguration, string>(new NoConfiguration(), "");
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

        private static ImmutableArray<StringPair> GetValues(
            AppSettingsBuilder appSettingsBuilder,
            string key,
            Action<string> logAction)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return ImmutableArray<StringPair>.Empty;
            }

            if (appSettingsBuilder == null)
            {
                return ImmutableArray<StringPair>.Empty;
            }

            ImmutableArray<StringPair> values = appSettingsBuilder.KeyValueConfiguration.AllValues;

            if (values.IsDefaultOrEmpty)
            {
                logAction?.Invoke(
                    $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} does not have any values for key '{key}'");

                return GetValues(appSettingsBuilder.Previous, key, logAction);
            }

            logAction?.Invoke(
                $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} has valuea for key '{key}': {string.Join(", ", values.Select(value => $"'{value}'"))}");

            return values;
        }

        private IKeyValueConfiguration GetConfiguratorDefining(AppSettingsBuilder appSettingsBuilder, string key)
        {
            if (appSettingsBuilder == null)
            {
                return new NoConfiguration();
            }

            if (appSettingsBuilder.KeyValueConfiguration.AllKeys.Contains(key, StringComparer.OrdinalIgnoreCase))
            {
                return appSettingsBuilder.KeyValueConfiguration;
            }

            return GetConfiguratorDefining(appSettingsBuilder.Previous, key);
        }

        private ImmutableArray<MultipleValuesStringPair> GetMultipleValues(AppSettingsBuilder appSettingsBuilder, ImmutableArray<string> keysLeft)
        {
            if (appSettingsBuilder == null)
            {
                return ImmutableArray<MultipleValuesStringPair>.Empty;
            }

            List<MultipleValuesStringPair> values =
                appSettingsBuilder.KeyValueConfiguration.AllWithMultipleValues
                .Where(multipleValuesStringPair => keysLeft.Any(keyLeft => keyLeft.Equals(multipleValuesStringPair.Key, StringComparison.OrdinalIgnoreCase)))
                .Where(multipleValuesStringPair => multipleValuesStringPair.HasNonEmptyValue)
                .ToList();

            if (values.Count == 0)
            {
                _logAction?.Invoke(
                    $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} does not have any values for multiple values");

                return GetMultipleValues(appSettingsBuilder.Previous, keysLeft);
            }

            ImmutableArray<string> keysLeftAfterValues = keysLeft.Except(values.Select(t => t.Key)).ToImmutableArray();
            if (keysLeftAfterValues.Any())
            {
                values.AddRange(GetMultipleValues(appSettingsBuilder.Previous, keysLeftAfterValues));
            }

            string FormatValue(MultipleValuesStringPair pair)
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

            if (appSettingsBuilder.Previous != null)
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

        public override string ToString()
        {
            return $"{base.ToString()} [{SourceChain}]";
        }
    }
}
