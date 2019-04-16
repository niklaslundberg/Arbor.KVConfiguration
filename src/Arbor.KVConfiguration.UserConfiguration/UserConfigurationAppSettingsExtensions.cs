using System;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.UserConfiguration
{
    public static class UserConfigurationAppSettingsExtensions
    {
        public static AppSettingsBuilder AddUserSettings([NotNull] this AppSettingsBuilder builder, string basePath)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrWhiteSpace(basePath))
            {
                return builder;
            }

            return builder.Add(new UserJsonConfiguration(basePath));
        }
    }
}
