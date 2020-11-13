namespace Arbor.KVConfiguration.Core.Decorators
{
    internal sealed class DecoratorDelegator : AppSettingsDecoratorBuilder
    {
        public DecoratorDelegator(AppSettingsBuilder appSettingsBuild) : base(appSettingsBuild, new NullDecorator())
        {
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose();
        }
    }
}