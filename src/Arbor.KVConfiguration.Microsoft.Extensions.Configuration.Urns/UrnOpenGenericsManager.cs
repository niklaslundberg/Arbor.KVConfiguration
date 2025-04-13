using System;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns;

public class UrnOpenGenericsManager<TOptions> : IConfigurationValue<TOptions> where TOptions : class
{
    private readonly OptionsCache<TOptions> _cache;

    public UrnOpenGenericsManager(IConfigureConfigurationValue<TOptions> configurator)
    {
        configurator.ThrowIfNull(nameof(configurator));

        _cache = new OptionsCache<TOptions>(configurator);
    }

    public TOptions Value => _cache?.Value ?? throw new InvalidOperationException("Could not get value");
}