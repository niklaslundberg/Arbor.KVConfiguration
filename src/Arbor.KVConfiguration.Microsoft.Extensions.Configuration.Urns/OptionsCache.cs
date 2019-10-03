using System;
using System.Threading;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    internal sealed class OptionsCache<TOptions> where TOptions : class
    {
        private readonly IConfigureConfigurationValue<TOptions> _configurator;
        private readonly Func<TOptions> _createCache;
        private bool _cacheInitialized;
        private object _cacheLock = new object();
        private TOptions? _options;

        public OptionsCache(IConfigureConfigurationValue<TOptions> configurator)
        {
            _configurator = configurator;
            _createCache = CreateOptions;
        }

        public TOptions? Value => LazyInitializer.EnsureInitialized(
            ref _options,
            ref _cacheInitialized,
            ref _cacheLock,
            _createCache);

        private TOptions CreateOptions() => _configurator.GetInstance();
    }
}
