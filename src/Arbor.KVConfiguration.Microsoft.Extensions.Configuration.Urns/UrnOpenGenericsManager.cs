using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public class UrnOpenGenericsManager<TOptions> : IConfigurationValue<TOptions> where TOptions : class
    {
        private readonly OptionsCache<TOptions> _cache;

        public UrnOpenGenericsManager([NotNull] IConfigureConfigurationValue<TOptions> configurator)
        {
            if (configurator is null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            _cache = new OptionsCache<TOptions>(configurator);
        }

        public TOptions Value => _cache.Value;
    }
}
