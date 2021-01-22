using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Extensions.ReflectionExtensions
{
    public static class ReflectionAppSettingsExtensions
    {
        public static AppSettingsBuilder AddReflectionSettings(
            [NotNull] this AppSettingsBuilder appSettingsBuilder,
            IEnumerable<Assembly>? scanAssemblies,
            Action<Exception>? exceptionHandler = null)
        {
            if (appSettingsBuilder is null)
            {
                throw new ArgumentNullException(nameof(appSettingsBuilder));
            }

            if (scanAssemblies is null)
            {
                return appSettingsBuilder;
            }

            foreach (var currentAssembly in scanAssemblies.OrderBy(assembly => assembly.FullName))
            {
                if (currentAssembly.IsDynamic)
                {
                    continue;
                }

                try
                {
                    appSettingsBuilder =
                        appSettingsBuilder.Add(
                            new ReflectionKeyValueConfiguration(currentAssembly));
                }
                catch (FileLoadException ex)
                {
                    exceptionHandler?.Invoke(ex);
                }
                catch (TypeLoadException ex)
                {
                    exceptionHandler?.Invoke(ex);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exceptionHandler?.Invoke(ex);
                }
            }

            return appSettingsBuilder;
        }
    }
}