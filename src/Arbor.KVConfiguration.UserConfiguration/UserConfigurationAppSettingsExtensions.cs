using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.UserConfiguration;

public static class UserConfigurationAppSettingsExtensions
{
    public static AppSettingsBuilder AddUserSettings(this AppSettingsBuilder builder, string basePath)
    {
        builder.ThrowIfNull(nameof(builder));

        if (string.IsNullOrWhiteSpace(basePath))
        {
            return builder;
        }

        return builder.Add(new UserJsonConfiguration(basePath));
    }
}