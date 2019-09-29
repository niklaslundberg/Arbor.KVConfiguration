using System;
using JetBrains.Annotations;

namespace Arbor.KVConfiguration.Core.Decorators
{
    public class AppSettingsDecoratorBuilder : IDisposable
    {
        private bool _isDisposed;

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

        public AppSettingsDecoratorBuilder? Previous { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing && !_isDisposed)
            {
                Previous?.Dispose();
                AppSettingsBuilder?.Dispose();
                _isDisposed = true;
            }
        }

        ~AppSettingsDecoratorBuilder() => Dispose(false);
    }
}
