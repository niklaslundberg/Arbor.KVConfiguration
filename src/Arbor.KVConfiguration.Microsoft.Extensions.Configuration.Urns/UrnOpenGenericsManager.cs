using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns
{
    public class UrnOpenGenericsManager<TOptions> : IConfigurationValue<TOptions> where TOptions : class
    {
        public UrnOpenGenericsManager([NotNull] IConfigureConfigurationValue<TOptions> configurator)
        {
            Value = configurator?.Configure() ?? throw new ArgumentNullException(nameof(configurator));
        }

        public TOptions Value { get; }
    }
}
