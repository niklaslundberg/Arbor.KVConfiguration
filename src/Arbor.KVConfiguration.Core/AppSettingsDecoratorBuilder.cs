using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core
{
    public class AppSettingsDecoratorBuilder
    {
        public AppSettingsDecoratorBuilder(
            [NotNull] AppSettingsBuilder appSettingsBuilder,
            [NotNull] IKeyValueConfigurationDecorator decorator)
        {
            AppSettingsBuilder = appSettingsBuilder ?? throw new ArgumentNullException(nameof(appSettingsBuilder));
            Decorator = decorator ?? throw new ArgumentNullException(nameof(decorator));
        }

        public AppSettingsDecoratorBuilder(
            [NotNull] AppSettingsDecoratorBuilder builder,
            [NotNull] IKeyValueConfigurationDecorator decorator)
        {
            Decorator = decorator ?? throw new ArgumentNullException(nameof(decorator));
            Previous = builder ?? throw new ArgumentNullException(nameof(builder));
            AppSettingsBuilder = Previous.AppSettingsBuilder;
        }

        public AppSettingsBuilder AppSettingsBuilder { get; }

        public IKeyValueConfigurationDecorator Decorator { get; }

        public AppSettingsDecoratorBuilder Previous { get; }
    }
}
