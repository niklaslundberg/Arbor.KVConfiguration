using Arbor.KVConfiguration.Core;
using Microsoft.Extensions.Configuration;

namespace Arbor.KVConfiguration.Microsoft.Extensions.Configuration.Urns;

public static class KeyValueConfigurationMicrosoftExtensions
{
    public static IConfigurationBuilder AddKeyValueConfigurationSource(
        this IConfigurationBuilder builder,
        IKeyValueConfiguration keyValueConfiguration)
    {
        builder.ThrowIfNull(nameof(builder));

        return builder.Add(new KeyValueConfigurationSourceAdapter(keyValueConfiguration));
    }
}