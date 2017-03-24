using System;
using System.Threading;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    internal class OptionsCache<TOptions> where TOptions : class
    {
        private readonly IConfigureConfigurationValue<TOptions> _configurator;
        private readonly Func<TOptions> _createCache;
        private object _cacheLock = new object();
        private bool _cacheInitialized;
        private TOptions _options;

        public OptionsCache(IConfigureConfigurationValue<TOptions> configurator)
        {
            _configurator = configurator;
            _createCache = CreateOptions;
        }

        private TOptions CreateOptions()
        {
            return _configurator.Configure();
        }

        public virtual TOptions Value
        {
            get
            {
                return LazyInitializer.EnsureInitialized(
                    ref _options,
                    ref _cacheInitialized,
                    ref _cacheLock,
                    _createCache);
            }
        }
    }
}