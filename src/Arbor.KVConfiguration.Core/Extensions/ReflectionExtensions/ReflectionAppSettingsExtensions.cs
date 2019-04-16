using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions
{
    public static class ReflectionAppSettingsExtensions
    {
        public static AppSettingsBuilder AddReflectionSettings(
            [NotNull] this AppSettingsBuilder appSettingsBuilder,
            IEnumerable<Assembly> scanAssemblies)
        {
            if (appSettingsBuilder == null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuilder));
            }

            if (scanAssemblies is null)
            {
                return appSettingsBuilder;
            }

            foreach (Assembly currentAssembly in scanAssemblies.OrderBy(assembly => assembly.FullName))
            {
                appSettingsBuilder =
                    appSettingsBuilder.Add(
                        new ReflectionKeyValueConfiguration(currentAssembly));
            }

            return appSettingsBuilder;
        }
    }
}
