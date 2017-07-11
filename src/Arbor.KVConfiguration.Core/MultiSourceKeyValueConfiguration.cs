using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public class MultiSourceKeyValueConfiguration : IKeyValueConfiguration
    {
        private readonly AppSettingsDecoratorBuilder _appSettingsDecoratorBuilder;
        private readonly Action<string> _logAction;

        public MultiSourceKeyValueConfiguration(
            [NotNull] AppSettingsDecoratorBuilder appSettingsDecoratorBuilder,
            Action<string> logAction = null)
        {
            _appSettingsDecoratorBuilder = appSettingsDecoratorBuilder ??
                                           throw new ArgumentNullException(nameof(appSettingsDecoratorBuilder));
            _logAction = logAction;
        }

        public ImmutableArray<string> AllKeys => GetAllKeys(_appSettingsDecoratorBuilder.AppSettingsBuilder)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToImmutableArray();

        public ImmutableArray<StringPair> AllValues
        {
            get
            {
                IEnumerable<StringPair> stringPairs = AllKeys.Select(key => new StringPair(key,
                    GetValue(_appSettingsDecoratorBuilder.AppSettingsBuilder, key, _logAction)));
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
                    GetMultipleValues(_appSettingsDecoratorBuilder.AppSettingsBuilder);

                ImmutableArray<MultipleValuesStringPair> multipleValuesStringPairs = values
                    .Select(item => new MultipleValuesStringPair(item.Key,
                        item.Values.Select(value => DecorateValue(_appSettingsDecoratorBuilder, value))
                            .ToImmutableArray()))
                    .ToImmutableArray();

                return multipleValuesStringPairs;
            }
        }

        public string this[string key] => DecorateValue(_appSettingsDecoratorBuilder,
            GetValue(_appSettingsDecoratorBuilder.AppSettingsBuilder, key, _logAction));

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

        private static string GetValue(AppSettingsBuilder appSettingsBuilder, string key, Action<string> logAction)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return "";
            }

            if (appSettingsBuilder == null)
            {
                return "";
            }

            string value =
                appSettingsBuilder.KeyValueConfiguration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                logAction?.Invoke(
                    $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} does not have a value for key '{key}'");

                return GetValue(appSettingsBuilder.Previous, key, logAction);
            }

            logAction?.Invoke(
                $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} has a value for key '{key}': '{value}'");

            return value;
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

        private ImmutableArray<MultipleValuesStringPair> GetMultipleValues(AppSettingsBuilder appSettingsBuilder)
        {
            if (appSettingsBuilder == null)
            {
                return ImmutableArray<MultipleValuesStringPair>.Empty;
            }

            ImmutableArray<MultipleValuesStringPair> values =
                appSettingsBuilder.KeyValueConfiguration.AllWithMultipleValues;

            if (values.IsDefaultOrEmpty)
            {
                _logAction?.Invoke(
                    $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} does not have any values for multiple values");

                return GetMultipleValues(appSettingsBuilder.Previous);
            }

            string FormatValue(MultipleValuesStringPair pair)
            {
                return $"\'{pair.Key}\'{string.Join("; ", pair.Values.Select(theValue => $"'{theValue}'"))}";
            }

            string join = string.Join(", ", values.Select(FormatValue));

            _logAction?.Invoke(
                $"The current source {appSettingsBuilder.KeyValueConfiguration.GetType().Name} has values: {join}");

            return values;
        }
    }
}
