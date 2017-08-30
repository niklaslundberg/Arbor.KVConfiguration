using System;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public static class KeyValueConfigurationBinder
    {
        public static TOptions Bind<TOptions>([NotNull] IConfiguration config) where TOptions : class
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var adapter = new KeyValueConfigurationAdapter(config);

            return adapter.GetInstance<TOptions>();
        }
    }
}
